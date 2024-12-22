using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.API.Contracts.Pet;

public record AddPetRequest(
    PetsNameDto Name, string SpecieName,
    string BreedName, string Gender, 
    DescriptionDto Description,
    string Color, HealthInformationDto HealthInformation,
    LocationAddressDto LocationAddress,
    double Weight, double Height,
    PhoneNumberDto OwnersPhoneNumber, bool IsSterilized,
    bool IsVaccinated, string CurrentStatus,
    List<DonationInfoDto>? DonateForHelpInfos,
    DateTime PetsPageCreationDate)

{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, Name, SpecieName, BreedName, 
            Gender, Description, Color, HealthInformation, 
            LocationAddress, Weight, Height,OwnersPhoneNumber, 
            IsSterilized, IsVaccinated, CurrentStatus, 
            DonateForHelpInfos, PetsPageCreationDate);
}