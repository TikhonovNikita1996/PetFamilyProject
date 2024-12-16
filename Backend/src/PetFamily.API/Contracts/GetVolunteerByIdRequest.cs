using PetFamily.Application.Queries.GetAllVolunteers;
using PetFamily.Application.Queries.GetVolunteerById;

namespace PetFamily.API.Contracts
{
    public record GetVolunteerByIdRequest(
        Guid? VolunteerId)
    {
        public GetVolunteerByIdQuery ToQuery() =>
            new (VolunteerId);
    }
}