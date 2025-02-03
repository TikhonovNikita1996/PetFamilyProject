using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.StartUploadPhotosToPet;

public class AddPhotosToPetHandler : ICommandHandler<IEnumerable<StartUploadFileResponse>,
    StartUploadPhotosToPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<StartUploadPhotosToPetCommand> _validator;
    private readonly IFileService _fileService;

    public AddPhotosToPetHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<StartUploadPhotosToPetCommand> validator,
        IFileService fileService)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _fileService = fileService;
    }

    public async Task<Result<IEnumerable<StartUploadFileResponse>, CustomErrorsList>> Handle(
        StartUploadPhotosToPetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        List<StartUploadFileResponse> fileResponses = [];
        
        foreach (var file in command.Files)
        {
            var validateResult = Photo.Validate(
                file.FileName,
                file.ContentType,
                file.FileSize);

            if (validateResult.IsFailure)
                return validateResult.Error.ToErrorList();
            
            var startMultipartRequest = new StartMultipartUploadRequest(
                file.FileName,
                file.ContentType,
                file.FileSize);
            
            var result = await _fileService.StartMultipartUpload(
                startMultipartRequest,
                cancellationToken);
            
            if (result.IsFailure)
                return Errors.General.ValueIsInvalid(result.Error).ToErrorList();

            var response = new StartUploadFileResponse(result.Value.FileId, result.Value.PresignedUrl);

            fileResponses.Add(response);
        }
        return fileResponses;
    }
}