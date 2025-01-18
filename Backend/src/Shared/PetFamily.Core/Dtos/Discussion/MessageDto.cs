namespace PetFamily.Core.Dtos.Discussion;

public class MessageDto
{
    public Guid MessageId { get; private set; }
    public string MessageText { get; private set; }
    public Guid SenderId { get; private set; } 
    public Guid DiscussionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;
}
