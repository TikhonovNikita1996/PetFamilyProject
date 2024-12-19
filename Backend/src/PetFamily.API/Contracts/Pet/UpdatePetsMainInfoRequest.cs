using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Pet;

public record UpdatePetsMainInfoRequest (Guid PetId, 
    UpdatePetsMainInfoDto Dto );