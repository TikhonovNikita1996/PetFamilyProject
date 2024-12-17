using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetAllVolunteers;

public record GetAllVolunteersQuery(
    Guid? VolunteerId,
    int Page,
    int PageSize) : IQuery;