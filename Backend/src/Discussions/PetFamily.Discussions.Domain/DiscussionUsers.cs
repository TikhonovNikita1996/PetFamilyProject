namespace PetFamily.Discussions.Domain;

public class DiscussionUsers
{
    public Guid FirstUserId { get; set; }
    public Guid SecondUserId { get; set; }
    
    private DiscussionUsers() { }

    public DiscussionUsers(Guid firstUserId, Guid secondUserId)
    {
        FirstUserId = firstUserId;
        SecondUserId = secondUserId;
    }
    
    public static DiscussionUsers Create(Guid firstUserId, Guid secondUserId) => 
        new DiscussionUsers(firstUserId, secondUserId);
}