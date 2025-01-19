namespace PetFamily.Core.Dtos.Discussion;

public class DiscussionDto
{
    public Guid DiscussionId { get; private set; }
    public Guid ReviewingUserId { get; private set;}
    public Guid ApplicantUserId { get; private set;}
    // public string Status { get; private set; } = default!;
    // public MessageDto[] Messages { get; private set; } = [];
}
