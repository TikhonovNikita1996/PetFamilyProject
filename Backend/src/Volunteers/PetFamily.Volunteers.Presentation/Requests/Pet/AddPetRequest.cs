using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPet;

namespace PetFamily.Volunteers.Presentation.Requests.Pet;

public record AddPetRequest(
    PetsNameDto Name, string SpecieName,
    int Age,
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
        new(volunteerId, Name, Age,  SpecieName, BreedName, 
            Gender, Description, Color, HealthInformation, 
            LocationAddress, Weight, Height,OwnersPhoneNumber, 
            IsSterilized, IsVaccinated, CurrentStatus, 
            DonateForHelpInfos, PetsPageCreationDate);
}