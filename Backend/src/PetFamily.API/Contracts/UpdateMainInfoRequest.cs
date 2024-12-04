using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts;
public record UpdateMainInfoRequest (Guid VolunteerId, 
    UpdateMainInfoDto Dto );