using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByAdminId;

public record GetAllRequestsByAdminIdQuery(
    Guid AdminId,
    string? Status,
    int Page,
    int PageSize) : IQuery;