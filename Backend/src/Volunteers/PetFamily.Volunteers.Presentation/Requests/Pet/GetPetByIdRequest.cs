using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;

namespace PetFamily.Volunteers.Presentation.Requests.Pet
{
    public record GetPetByIdRequest(
        Guid PetId)
    {
        public GetPetByIdQuery ToQuery() =>
            new (PetId);
    }
}