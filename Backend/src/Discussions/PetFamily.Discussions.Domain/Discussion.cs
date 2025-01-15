using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Discussions.Domain;

public class Discussion
{
    public Guid Id { get; private set; }
    public Guid RelationId { get; private set; }
    public Relation Relation { get; private set; }
    public List<Guid> UserIds { get; private set; } = [];
    public List<Message> Messages { get; private set; } = [];
    public DiscussionStatus Status { get; private set; }
    
    private Discussion() { }
    
    private Discussion(Guid relationId, IEnumerable<Guid> userIds)
    {
        Id = Guid.NewGuid();
        RelationId = relationId;
        UserIds = userIds.ToList();
        Status = DiscussionStatus.Open;
    }
    
    public void Close() => Status = DiscussionStatus.Closed;
    public void Reopen() => Status = DiscussionStatus.Open;
    
    public static Result<Discussion, CustomError> Create(
        Guid relationId, IEnumerable<Guid> userIds)
    {
        if (userIds.Count() != 2)
            return Errors.General.Failure("There must be 2 users in discussion.");

        return new Discussion(relationId, userIds);
    }
    
    public UnitResult<CustomError> AddMessage(Message message)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.General.Failure("Discussion is closed.");

        bool isUserInCurrentDiscussion = UserIds.Contains(message.SenderId);

        if (isUserInCurrentDiscussion is not true)
            return Errors.General.Failure("User is not in this discussion.");

        Messages.Add(message);
        return Result.Success<CustomError>();
    }
    
    public UnitResult<CustomError> RemoveMessage(Message message, Guid userId)
    {
        if (Status == DiscussionStatus.Closed)
            return Errors.General.Failure("Discussion is closed.");

        if (message.SenderId != userId)
            return Errors.General.Failure("You can delete only your messages.");
        
        Messages.Remove(message);
        return Result.Success<CustomError>();
    }
}