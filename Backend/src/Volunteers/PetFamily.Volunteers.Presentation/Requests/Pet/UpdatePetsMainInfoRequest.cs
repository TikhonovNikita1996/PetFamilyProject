using PetFamily.Core.Dtos.Pet;

namespace PetFamily.Volunteers.Presentation.Requests.Pet;

public record UpdatePetsMainInfoRequest (Guid PetId, 
    UpdatePetsMainInfoDto Dto );