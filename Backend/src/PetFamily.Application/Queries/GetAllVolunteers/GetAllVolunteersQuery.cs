using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Queries.GetAllVolunteers;

public record GetAllVolunteersQuery(
    int Page,
    int PageSize) : IQuery;