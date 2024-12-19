using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Volunteer;
public record UpdateMainInfoRequest (Guid VolunteerId, 
    UpdateVolunteerMainInfoDto Dto );