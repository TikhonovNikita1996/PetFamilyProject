using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetVolunteerById;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer
{
    public record GetVolunteerByIdRequest(
        Guid? VolunteerId)
    {
        public GetVolunteerByIdQuery ToQuery() =>
            new (VolunteerId);
    }
}