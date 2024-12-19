﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.HardPetDelete;

public class HardPetDeleteHandler : ICommandHandler<Guid,HardPetDeleteCommand>
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly IValidator<HardPetDeleteCommand> _validator;
    private readonly ILogger<HardPetDeleteHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileProvider _fileProvider;

    public HardPetDeleteHandler(
        ILogger<HardPetDeleteHandler> logger,
        IVolunteerRepository volunteersRepository,
        IValidator<HardPetDeleteCommand> validator,
        IUnitOfWork unitOfWork,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _fileProvider = fileProvider;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(HardPetDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petToDelete = volunteerResult.Value.CurrentPets.FirstOrDefault(p => p.Id == command.PetId);
        if (petToDelete == null)
            return volunteerResult.Error.ToErrorList();
        
        volunteerResult.Value.DeletePet(petToDelete);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        // Delete photos from minio
        
        var photosMetaDataToDelete = petToDelete.PhotosList.PetPhotos
            .Select(p => new FileMetaData("photos", FilePath.Create(p.FilePath).Value));

        foreach (var photoMetaData in photosMetaDataToDelete)
        {
            await _fileProvider.DeleteFileAsync(photoMetaData, cancellationToken);
        }
        
        _logger.LogInformation("Pet with Id: {id} was deleted", command.PetId);
        
        return command.PetId;
    }
}