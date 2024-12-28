using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetAllSpecies;

public record GetAllSpeciesQuery(
    int Page,
    int PageSize) : IQuery;