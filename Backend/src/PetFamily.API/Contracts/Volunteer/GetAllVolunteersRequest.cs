using PetFamily.Application.Queries.GetAllVolunteers;

namespace PetFamily.API.Contracts.Volunteer
{
    public record GetAllVolunteersRequest(
        int Page,
        int PageSize)
    {
        public GetAllVolunteersQuery ToQuery() =>
            new ( Page, PageSize);
    }
}