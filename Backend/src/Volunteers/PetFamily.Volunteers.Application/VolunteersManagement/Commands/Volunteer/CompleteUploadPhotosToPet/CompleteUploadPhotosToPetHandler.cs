using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.CompleteUploadPhotosToPet;

public class CompleteUploadUserAvatarHandler : ICommandHandler<CompleteUploadPhotosToPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public CompleteUploadUserAvatarHandler(
        IVolunteerRepository volunteerRepository,
        [FromKeyedServices(ProjectConstants.Context.AccountManagement)] IUnitOfWork unitOfWork,
        IFileService fileService)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<UnitResult<CustomErrorsList>> Handle(
        CompleteUploadPhotosToPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        List<Guid> photosResultsList = [];
        
        foreach (var dto in command.Dtos)
        {
            var validateResult = Photo.Validate(
                dto.FileName,
                dto.ContentType,
                dto.FileSize);

            if (validateResult.IsFailure)
                return validateResult.Error.ToErrorList();
            
            var completeRequest = new CompleteMultipartRequest(dto.UploadId, dto.Parts);
        
            var result = await _fileService.CompleteMultipartUpload(completeRequest, cancellationToken);
        
            if (result.IsFailure)
                return Errors.General.Failure(result.Error).ToErrorList();

            photosResultsList.Add(result.Value.FileId);
        }
        
        List<Photo> photos = photosResultsList.Select((t, i) => i == 0
            ? new Photo(t, true)
            : new Photo(t, false))
        .ToList();

        petResult.Value.UpdatePhotos(photos);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return UnitResult.Success<CustomErrorsList>();
    }
}