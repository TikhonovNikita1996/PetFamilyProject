using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetAllVolunteers;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer
{
    public record GetAllVolunteersRequest(
        int Page,
        int PageSize)
    {
        public GetAllVolunteersQuery ToQuery() =>
            new ( Page, PageSize);
    }
}