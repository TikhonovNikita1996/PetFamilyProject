using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetBreedByName;

public record GetBreedByNameQuery(string BreedName) : IQuery;