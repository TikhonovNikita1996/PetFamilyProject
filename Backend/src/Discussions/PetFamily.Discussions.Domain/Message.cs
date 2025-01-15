namespace PetFamily.Discussions.Domain;

public class Message
{
    public Guid MessageId { get; private set; }
    public MessageText MessageText { get; private set; }
    public Guid SenderId { get; private set; } 
    public Guid DiscussionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsEdited { get; private set; } = false;

    private Message() { }

    private Message(MessageText text, Guid senderId, Guid discussionId)
    {
        MessageId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        MessageText = text;
        SenderId = senderId;
        DiscussionId = discussionId;
    }

    public static Message Create(MessageText text, Guid senderId, Guid discussionId) 
        => new Message(text, senderId, discussionId);

    public void EditMessage(MessageText newText)
    {
        MessageText = newText;
        IsEdited = true;
    }
}