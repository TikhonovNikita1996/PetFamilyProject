using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.SpecieBreeds;

public record GetSpecieByIdQuery(Guid BreedId) : IQuery;