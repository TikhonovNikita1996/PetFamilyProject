using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;
using Description = PetFamily.Domain.Entities.Pet.ValueObjects.Description;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteerRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
        var volunteerResult = await _volunteersRepository.GetById(
            volunteerId, cancellationToken);
        
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.NewId().Value;
        var petsName = PetsName.Create(command.Name.Name).Value;
        var gender  = command.Gender;
        var description = Description.Create(command.Description.Value).Value;
        var color = command.Color;
        var healthInformation = HealthInformation.Create(command.HealthInformation.Value).Value;
        var height = command.Height;
        var weight = command.Weight;
        var ownersPhoneNumber = OwnersPhoneNumber.Create(command.OwnersPhoneNumber.Value).Value;
        var isSterilized = command.IsSterilized;
        var isVaccinated = command.IsVaccinated;
        var helpStatus = Enum.Parse<HelpStatusType>(command.CurrentStatus);
        var dateTime = command.PetsPageCreationDate;
        var locationAddress = LocationAddress.Create(command.LocationAddress.Region,
            command.LocationAddress.City, command.LocationAddress.Street, command.LocationAddress.HouseNumber,
            command.LocationAddress.Floor, command.LocationAddress.Apartment).Value;

        var specieName = command.SpecieName;
        
        
        

        await _unitOfWork.SaveChanges(cancellationToken);
        _logger.LogInformation("Pet added with id: {PetId}.", pet.Id.Value);

        return pet.Id.Value;
    }
}