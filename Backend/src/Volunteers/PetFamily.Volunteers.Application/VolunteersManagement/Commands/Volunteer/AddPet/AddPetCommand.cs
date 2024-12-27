using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    PetsNameDto Name, int Age,
    string SpecieName,
    string BreedName, string Gender, 
    DescriptionDto Description,
    string Color, HealthInformationDto HealthInformation,
    LocationAddressDto LocationAddress,
    double Weight, double Height,
    PhoneNumberDto OwnersPhoneNumber, bool IsSterilized,
    bool IsVaccinated, string CurrentStatus,
    List<DonationInfoDto>? DonateForHelpInfos,
    DateTime PetsPageCreationDate) : ICommand;