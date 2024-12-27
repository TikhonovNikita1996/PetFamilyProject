using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetBreedsBySpecieId;

public record GetBreedsBySpecieIdQuery(
    Guid SpecieId,
    int Page,
    int PageSize) : IQuery;