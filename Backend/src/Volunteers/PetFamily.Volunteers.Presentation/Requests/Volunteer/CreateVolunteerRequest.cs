using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record CreateVolunteerRequest(
    PhoneNumberDto PhoneNumber,
    DescriptionDto Description);