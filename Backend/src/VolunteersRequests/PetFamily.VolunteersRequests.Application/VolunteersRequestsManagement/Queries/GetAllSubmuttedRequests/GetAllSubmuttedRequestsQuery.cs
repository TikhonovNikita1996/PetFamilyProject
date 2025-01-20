using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllSubmuttedRequests;

public record GetAllSubmittedRequestsQuery(
    int Page,
    int PageSize) : IQuery;