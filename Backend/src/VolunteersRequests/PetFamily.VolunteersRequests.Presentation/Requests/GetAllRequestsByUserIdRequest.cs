namespace PetFamily.VolunteersRequests.Presentation.Requests
{
    public record GetAllRequestsByUserIdRequest(
        Guid UserId,
        string? Status,
        int Page,
        int PageSize);
}