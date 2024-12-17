using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid? VolunteerId) : IQuery;