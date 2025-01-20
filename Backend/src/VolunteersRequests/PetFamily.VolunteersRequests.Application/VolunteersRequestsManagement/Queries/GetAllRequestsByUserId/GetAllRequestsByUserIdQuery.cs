using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByUserId;

public record GetAllRequestsByUserIdQuery(
    Guid UserId,
    string? Status,
    int Page,
    int PageSize) : IQuery;