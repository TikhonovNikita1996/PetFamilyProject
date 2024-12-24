using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;