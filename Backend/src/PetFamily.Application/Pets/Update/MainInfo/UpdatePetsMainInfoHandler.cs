﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.Update.MainInfo;

public class UpdatePetsMainInfoHandler : ICommandHandler<Guid,UpdatePetsMainInfoCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdatePetsMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetsMainInfoCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public UpdatePetsMainInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdatePetsMainInfoHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<UpdatePetsMainInfoCommand> validator,
        IReadDbContext readDbContext)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(UpdatePetsMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var specieQuery = _readDbContext.Species.AsQueryable();
        var specieDto = await specieQuery
            .SingleOrDefaultAsync(p => p.SpecieId == command.Dto.SpeciesId, cancellationToken);
        if (specieDto == null)
            return Errors.General.NotFound("specie").ToErrorList();
        
        var breedQuery = _readDbContext.Breeds.AsQueryable();
        var breedDto = await breedQuery
            .SingleOrDefaultAsync(p => p.SpecieId == command.Dto.SpeciesId &&
                p.BreedId == command.Dto.BreedId, cancellationToken);
        
        if (breedDto == null)
            return Errors.General.NotFound("breed").ToErrorList();
        
        var specieBreed = SpecieDetails.Create(SpecieId.Create(command.Dto.SpeciesId),
            BreedId.Create(command.Dto.BreedId)).Value;
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var name = PetsName.Create(command.Dto.Name).Value;
        var gender = Enum.Parse<GenderType>(command.Dto.Gender);
        var description = PetsDescription.Create(command.Dto.Description).Value;
        var color = Color.Create(command.Dto.Color).Value;
        var phoneNumber = OwnersPhoneNumber.Create(command.Dto.OwnersPhoneNumber).Value;

        var locationAddress = LocationAddress.Create(
            command.Dto.LocationAddressDto.Region,
            command.Dto.LocationAddressDto.City,
            command.Dto.LocationAddressDto.Street,
            command.Dto.LocationAddressDto.HouseNumber,
            command.Dto.LocationAddressDto.Floor,
            command.Dto.LocationAddressDto.Apartment).Value;

        var petToUpdate = volunteerResult.Value.CurrentPets.FirstOrDefault(p => p.Id == command.PetId);
        
        if(petToUpdate == null)
            return Errors.General.NotFound("pet").ToErrorList();
        
        petToUpdate.UpdateMainInfo(name, specieBreed,
            gender, description, locationAddress,
            command.Dto.Weight, command.Dto.Height,
            command.Dto.IsSterilized, command.Dto.IsVaccinated,
            phoneNumber, color);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("For pet with Id: {id} was updated main info", petToUpdate.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}