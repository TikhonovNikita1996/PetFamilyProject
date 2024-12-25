using PetFamily.Application.Queries.GetPetById;

namespace PetFamily.API.Contracts.Pet
{
    public record GetPetByIdRequest(
        Guid PetId)
    {
        public GetPetByIdQuery ToQuery() =>
            new (PetId);
    }
}