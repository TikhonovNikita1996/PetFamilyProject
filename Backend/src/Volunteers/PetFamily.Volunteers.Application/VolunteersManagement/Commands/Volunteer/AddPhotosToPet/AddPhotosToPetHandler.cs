using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Domain.Pet.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPhotosToPet;

public class AddPhotosToPetHandler : ICommandHandler<Guid,AddPhotosToPetCommand>
{
    private readonly ILogger<AddPhotosToPetHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddPhotosToPetCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileMetaData>> _messageQueue;
    private readonly IFileService _fileService;
    private const string BUCKET_NAME = "photos";

    public AddPhotosToPetHandler(
        ILogger<AddPhotosToPetHandler> logger,
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddPhotosToPetCommand> validator, 
        IMessageQueue<IEnumerable<FileMetaData>> messageQueue,
        IFileService fileService)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _messageQueue = messageQueue;
        _fileService = fileService;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(
        AddPhotosToPetCommand command,
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
        
        List<FileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileContent = new FileData(file.Stream,new FileMetaData(BUCKET_NAME,
                FilePath.Create(file.FileName).Value));

            filesData.Add(fileContent);
        }
        
        var filePathsResult = await _fileService.UploadFilesAsync(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.FileMetaData),cancellationToken);
            return filePathsResult.Error.ToErrorList();
        }

        List<PetPhoto> photos = filePathsResult.Value.Select((t, i) => i == 0
                ? PetPhoto.Create(t.Path, true).Value
                : PetPhoto.Create(t.Path, false).Value)
            .ToList();

        petResult.Value.UpdatePhotos(photos);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Uploaded photos to pet - {petId}", petResult.Value.Id.Value);
        
        return petResult.Value.Id.Value;
    }
}