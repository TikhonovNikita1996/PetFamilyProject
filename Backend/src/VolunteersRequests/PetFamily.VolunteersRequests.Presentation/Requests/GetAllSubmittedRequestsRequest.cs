namespace PetFamily.VolunteersRequests.Presentation.Requests
{
    public record GetAllSubmittedRequestsRequest(
        int Page,
        int PageSize);
}