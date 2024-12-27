using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetAllVolunteers;

public record GetAllVolunteersQuery(
    int Page,
    int PageSize) : IQuery;