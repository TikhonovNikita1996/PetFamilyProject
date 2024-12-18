using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetBreedsBySpecieId;

public record GetBreedsBySpecieIdQuery(
    Guid SpecieId,
    int Page,
    int PageSize) : IQuery;