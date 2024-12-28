using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetSpecieById;

public record GetSpecieByIdQuery(Guid SpecieId) : IQuery;