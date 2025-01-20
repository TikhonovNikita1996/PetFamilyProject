namespace PetFamily.Core.Dtos.VolunteersRequest;

public class VolunteersRequestDto
{
    public Guid RequestId { get; set; }
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public string VolunteerInfo { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? RejectionComment { get; private set; }
}
