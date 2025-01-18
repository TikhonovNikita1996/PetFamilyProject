using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Domain.Ids;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPet;

public class AddPetHandler : ICommandHandler<Guid,AddPetCommand>
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesContract _speciesContract;
    private readonly IRelationContracts _relationContracts;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteerRepository volunteersRepository,
        IValidator<AddPetCommand> validator,
        [FromKeyedServices(ProjectConstants.Context.VolunteerManagement)] IUnitOfWork unitOfWork,
        ISpeciesContract speciesContract,
        IRelationContracts relationContracts)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _speciesContract = speciesContract;
        _relationContracts = relationContracts;
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
        var petsAge = Age.Create(command.Age).Value;
        var gender  = Enum.Parse<GenderType>(command.Gender);
        var description = PetsDescription.Create(command.Description.Value).Value;
        var color = Color.Create(command.Color).Value;
        var healthInformation = HealthInformation.Create(command.HealthInformation.Value).Value;
        var height = command.Height;
        var weight = command.Weight;
        var ownersPhoneNumber = OwnersPhoneNumber.Create(command.OwnersPhoneNumber.Value).Value;
        var isSterilized = command.IsSterilized;
        var isVaccinated = command.IsVaccinated;
        var helpStatus = Enum.Parse<HelpStatusType>(command.CurrentStatus);
        var pageCreationDate = command.PetsPageCreationDate;
        var locationAddress = LocationAddress.Create(command.LocationAddress.Region,
            command.LocationAddress.City, command.LocationAddress.Street, command.LocationAddress.HouseNumber,
            command.LocationAddress.Floor, command.LocationAddress.Apartment).Value;

        var donationInfos = command.DonateForHelpInfos
            .Select(s => DonationInfo.Create(s.Name, s.Description));
        
        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());
        
        var specieDto = await _speciesContract.GetSpecieByName(new GetSpecieByNameRequest(command.SpecieName),
            cancellationToken);
        
        if (specieDto == null)
            return Errors.General.NotFound("specie").ToErrorList();

        var specieId = specieDto.SpecieId;
        
        var breedDto = await _speciesContract.GetBreedByName(new GetBreedByNameRequest(specieId,command.SpecieName),
            cancellationToken);
        
        if (breedDto == null)
            return Errors.General.NotFound("breed").ToErrorList();

        var breedId = breedDto.BreedId;
        
        var specieDetails = SpecieDetails.Create(SpecieId.Create(specieId), BreedId.Create(breedId)).Value;
        
        var newPet = Domain.Pet.Pet.Create(petId, petsName, petsAge, specieDetails,
            gender, description, color, healthInformation,
            locationAddress, weight, height, ownersPhoneNumber,
            isSterilized, isVaccinated, helpStatus, resultDonationInfoList, pageCreationDate).Value;
        
        volunteerResult.Value.AddPet(newPet);
        
        await _relationContracts.CreateRelation(petId, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet added with id: {PetId}.", newPet.Id.Value);
        return newPet.Id.Value;
    }
}