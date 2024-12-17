using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPhotosToPet;

public class AddPhotosToPetHandler : ICommandHandler<Guid,AddPhotosToPetCommand>
{
    private readonly ILogger<AddPhotosToPetHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPhotosToPetCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileMetaData>> _messageQueue;
    private readonly IFileProvider _fileProvider;
    private const string BUCKET_NAME = "photos";

    public AddPhotosToPetHandler(
        ILogger<AddPhotosToPetHandler> logger,
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        IValidator<AddPhotosToPetCommand> validator, 
        IMessageQueue<IEnumerable<FileMetaData>> messageQueue,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _messageQueue = messageQueue;
        _fileProvider = fileProvider;
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
        
        var filePathsResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.FileMetaData),cancellationToken);
            return filePathsResult.Error.ToErrorList();
        }
        
        var petPhotos = filePathsResult.Value
            .Select(f => PetPhoto.Create(f.Path, false).Value)
            .ToList();

        petResult.Value.UpdatePhotos(new PhotosList(petPhotos));
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Uploaded photos to pet - {petId}", petResult.Value.Id.Value);
        
        return petResult.Value.Id.Value;
    }
}