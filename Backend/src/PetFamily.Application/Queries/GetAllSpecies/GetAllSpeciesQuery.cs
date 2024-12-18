using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetAllSpecies;

public record GetAllSpeciesQuery(
    int Page,
    int PageSize) : IQuery;