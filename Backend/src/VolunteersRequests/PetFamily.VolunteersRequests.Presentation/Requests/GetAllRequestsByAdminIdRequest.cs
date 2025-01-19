namespace PetFamily.VolunteersRequests.Presentation.Requests
{
    public record GetAllRequestsByAdminIdRequest(
        Guid AdminId,
        string? Status,
        int Page,
        int PageSize);
}