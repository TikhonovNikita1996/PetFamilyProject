using PetFamily.Application.Queries.GetVolunteerById;

namespace PetFamily.API.Contracts.Volunteer
{
    public record GetVolunteerByIdRequest(
        Guid? VolunteerId)
    {
        public GetVolunteerByIdQuery ToQuery() =>
            new (VolunteerId);
    }
}