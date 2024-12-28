using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;
public record UpdateMainInfoRequest (Guid VolunteerId, 
    UpdateVolunteerMainInfoDto Dto );