using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Contracts;

public record AddPetRequest(
    PetsNameDto Name, string SpecieName,
    string BreedName, string Gender, 
    DescriptionDto Description,
    string Color, HealthInformationDto HealthInformation,
    LocationAddressDto LocationAddress,
    double Weight, double Height,
    PhoneNumberDto OwnersPhoneNumber, bool IsSterilized,
    DateTime DateOfBirth,
    bool IsVaccinated, string CurrentStatus,
    List<DonationInfoDto>? DonateForHelpInfos,
    DateTime PetsPageCreationDate)

{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, Name, SpecieName, BreedName, 
            Gender, Description, Color, HealthInformation, 
            LocationAddress, Weight, Height,OwnersPhoneNumber, 
            IsSterilized, DateOfBirth, IsVaccinated, CurrentStatus, 
            DonateForHelpInfos, PetsPageCreationDate);
}