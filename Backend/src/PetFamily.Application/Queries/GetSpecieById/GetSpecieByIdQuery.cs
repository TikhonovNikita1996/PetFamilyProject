using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetSpecieById;

public record GetSpecieByIdQuery(Guid SpecieId) : IQuery;