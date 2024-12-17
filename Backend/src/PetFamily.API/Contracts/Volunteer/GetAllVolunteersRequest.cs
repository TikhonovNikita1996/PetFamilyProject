using PetFamily.Application.Queries.GetAllVolunteers;

namespace PetFamily.API.Contracts.Volunteer
{
    public record GetAllVolunteersRequest(
        Guid? VolunteerId,
        int Page,
        int PageSize)
    {
        public GetAllVolunteersQuery ToQuery() =>
            new (VolunteerId, Page, PageSize);
    }
}