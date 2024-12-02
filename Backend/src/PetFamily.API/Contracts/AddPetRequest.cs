using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Contracts;

public record AddPetRequest(
    string Name, Guid SpeciesId,
    GenderType Gender, string Description,
    string Color, string HealthInformation,
    LocationAddress LocationAddress,
    double Weight, double Height,
    string OwnersPhoneNumber, bool IsSterilized,
    DateTime DateOfBirth,
    bool IsVaccinated, HelpStatusType CurrentStatus,
    List<DonationInfoDto>? DonateForHelpInfos,
    DateTime PetsPageCreationDate);