using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetsWithFiltersAndPagination;

public record GetPetsWithPaginationAndFiltersQuery(
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
    int PageSize) : IQuery;