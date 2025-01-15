using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Discussions.Domain;

public class Discussion
{
    private readonly List<Message> _messages = [];
    public Guid DiscussionId { get; private set; }
    public Guid RelationId { get; private set; }
    public Relation Relation { get; private set; }
    public DiscussionUsers DiscussionUsers { get; } = default!;
    public IReadOnlyList<Message> Messages => _messages;
    public DiscussionStatus Status { get; private set; }
    
    private Discussion() { }
    
    private Discussion(Guid relationId, DiscussionUsers discussionUsers)
    {
        DiscussionId = Guid.NewGuid();
        RelationId = relationId;
        DiscussionUsers = discussionUsers;
        Status = DiscussionStatus.Open;
    }
    
    public void Close() => Status = DiscussionStatus.Closed;
    public void Reopen() => Status = DiscussionStatus.Open;
    
    public static Result<Discussion, CustomError> Create(
        Guid relationId, DiscussionUsers discussionUsers)
    {
        return new Discussion(relationId, discussionUsers);
    }
    
    public UnitResult<CustomError> AddMessage(Message message)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.General.Failure("Discussion is closed.");

        bool isUserInCurrentDiscussion = DiscussionUsers.FirstUserId == message.SenderId ||
                                         DiscussionUsers.SecondUserId == message.SenderId;

        if (isUserInCurrentDiscussion is not true)
            return Errors.General.Failure("User is not in this discussion.");

        _messages.Add(message);
        return Result.Success<CustomError>();
    }
    
    public UnitResult<CustomError> EditMessage(Guid messageId, Guid userId, MessageText messageText)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.General.Failure("Discussion is closed.");

        var message = _messages.FirstOrDefault(m => m.MessageId == messageId);
        
        if (message.SenderId != userId)
            return Errors.General.Failure("You can edit only your messages.");
        
        message.EditMessage(messageText);
        
        return Result.Success<CustomError>();
    }
    
    public UnitResult<CustomError> RemoveMessage(Message message, Guid userId)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.General.Failure("Discussion is closed.");

        if (message.SenderId != userId)
            return Errors.General.Failure("You can delete only your messages.");
        
        _messages.Remove(message);
        return Result.Success<CustomError>();
    }
}