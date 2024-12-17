using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Application.PetsSpecies.AddBreed;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler : ICommandHandler<Guid,AddPetCommand>
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _readDbContext;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteerRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.NewId();
        var petsName = PetsName.Create(command.Name.Name).Value;
        var gender  = Enum.Parse<GenderType>(command.Gender);
        var description = PetsDescription.Create(command.Description.Value).Value;
        var color = Color.Create(command.Color).Value;
        var healthInformation = HealthInformation.Create(command.HealthInformation.Value).Value;
        var height = command.Height;
        var weight = command.Weight;
        var ownersPhoneNumber = OwnersPhoneNumber.Create(command.OwnersPhoneNumber.Value).Value;
        var isSterilized = command.IsSterilized;
        var isVaccinated = command.IsVaccinated;
        var dateOfBirth = command.DateOfBirth;
        var helpStatus = Enum.Parse<HelpStatusType>(command.CurrentStatus);
        var pageCreationDate = command.PetsPageCreationDate;
        var locationAddress = LocationAddress.Create(command.LocationAddress.Region,
            command.LocationAddress.City, command.LocationAddress.Street, command.LocationAddress.HouseNumber,
            command.LocationAddress.Floor, command.LocationAddress.Apartment).Value;

        var specieQuery = _readDbContext.Species;
        var specieDto = await specieQuery
            .SingleOrDefaultAsync(s => s.Name == command.SpecieName, cancellationToken);
        
        if (specieDto == null)
            return Errors.General.NotFound("specie").ToErrorList();
        
        var donationInfos = command.DonateForHelpInfos
            .Select(s => DonationInfo.Create(s.Name, s.Description));

        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());

        var specieId = specieDto.SpecieId;
        
        var breedQuery = _readDbContext.Breeds;
        var breedDto = await breedQuery
            .SingleOrDefaultAsync(b => b.Name == command.BreedName
                                       && b.SpecieId == specieId, cancellationToken);
        if (breedDto == null)
            return Errors.General.NotFound("breed").ToErrorList();

        var breedId = breedDto.BreedId;
        
        var specieDetails = SpecieDetails.Create(SpecieId.Create(specieId), BreedId.Create(breedId)).Value;
        
        var newPet = Pet.Create(petId, petsName, specieDetails,
            gender, description, color, healthInformation,
            locationAddress, weight, height, ownersPhoneNumber,
            isSterilized, dateOfBirth, isVaccinated, helpStatus, resultDonationInfoList, pageCreationDate).Value;
        
        volunteerResult.Value.AddPet(newPet);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet added with id: {PetId}.", newPet.Id.Value);
        return newPet.Id.Value;
    }
}