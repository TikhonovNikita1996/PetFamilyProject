using PetFamily.Application.Queries.GetAllVolunteers;
using PetFamily.Application.Queries.GetPetsWithFiltersAndPagination;

namespace PetFamily.API.Contracts.Pet
{
    public record GetPetsWithPaginationAndFiltersRequest(
        Guid? VolunteerId,
        string? Name,
        int? Age,
        string? Gender,
        Guid? SpeciesId,
        Guid? BreedId,
        string? Color,
        string? Status,
        string? SortBy,
        string? SortDirection,
        int Page,
        int PageSize)
    {
        public GetPetsWithPaginationAndFiltersQuery ToQuery() =>
            new ( VolunteerId, Name, Age, Gender, SpeciesId, BreedId, Color,
                Status, SortBy, SortDirection ,Page, PageSize);
    }
}