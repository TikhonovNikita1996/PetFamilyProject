using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.SpecieBreeds;

public record GetBreedByIdQuery(Guid BreedId) : IQuery;
