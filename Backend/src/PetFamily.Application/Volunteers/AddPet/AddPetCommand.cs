using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetCommand(
    string Name, Guid SpeciesID,
    GenderType Gender, string Description,
    string Color, string HealthInformation,
    LocationAddressDto LocationAddress,
    double Weight, double Height,
    string OwnersPhoneNumber, bool IsSterilized,
    DateTime DateOfBirth,
    bool IsVaccinated, HelpStatusType CurrentStatus,
    List<DonationInfoDto>? DonateForHelpInfos,
    DateTime PetsPageCreationDate);