namespace PetFamily.Discussions.Domain;

public class DiscussionUsers
{
    public Guid ReviewingUserId { get; set; }
    public Guid ApplicantUserId { get; set; }
    
    private DiscussionUsers() { }

    public DiscussionUsers(Guid applicantUserId, Guid reviewingUserId)
    {
        ReviewingUserId = reviewingUserId;
        ApplicantUserId = applicantUserId;
    }
    
    public static DiscussionUsers Create(Guid firstUserId, Guid secondUserId) => 
        new DiscussionUsers(firstUserId, secondUserId);
}