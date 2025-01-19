namespace PetFamily.Discussions.Domain;

public class DiscussionUsers
{
    public Guid ReviewingUserId { get; set; }
    public Guid ApplicantUserId { get; set; }
    
    private DiscussionUsers() { }

    private DiscussionUsers(Guid reviewingUserId, Guid applicantUserId)
    {
        ReviewingUserId = reviewingUserId;
        ApplicantUserId = applicantUserId;
    }
    
    public static DiscussionUsers Create(Guid reviewingUserId, Guid applicantUserId) => 
        new DiscussionUsers(reviewingUserId, applicantUserId);
}