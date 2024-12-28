using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;