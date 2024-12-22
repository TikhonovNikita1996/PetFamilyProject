using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    PetsNameDto Name, string SpecieName,
    string BreedName, string Gender, 
    DescriptionDto Description,
    string Color, HealthInformationDto HealthInformation,
    LocationAddressDto LocationAddress,
    double Weight, double Height,
    PhoneNumberDto OwnersPhoneNumber, bool IsSterilized,
    bool IsVaccinated, string CurrentStatus,
    List<DonationInfoDto>? DonateForHelpInfos,
    DateTime PetsPageCreationDate) : ICommand;