using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetSpecieByName;

public record GetSpecieByNameQuery(string SpecieName) : IQuery;