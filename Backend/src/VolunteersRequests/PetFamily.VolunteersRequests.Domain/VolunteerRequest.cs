using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain;

public class VolunteerRequest
{
    public Guid RequestId { get; set; }
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public RejectionComment? RejectionComment { get; private set; }
    
    private VolunteerRequest(
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        UserId = userId;
        RequestId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Status = RequestStatus.Submitted;
        VolunteerInfo = volunteerInfo;
    }
    
    public static Result<VolunteerRequest, CustomError>  Create(
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        var request = new VolunteerRequest(userId, volunteerInfo);
        return request;
    }
    
    public void TakeInReview(Guid adminId, Guid discussionId)
    {
        AdminId = adminId;
        DiscussionId = discussionId;
        Status = RequestStatus.OnReview;
    }
    
    public void SetRevisionRequiredStatus(
        Guid adminId,
        RejectionComment rejectedComment)
    {
        AdminId = adminId;
        Status = RequestStatus.RevisionRequired;
        RejectionComment = rejectedComment;
    }

    public void SetApprovedStatus(
        Guid adminId)
    {
        AdminId = adminId;
        Status = RequestStatus.Approved;
    }
    
    public void SetRejectStatus(
        Guid adminId)
    {
        AdminId = adminId;
        Status = RequestStatus.Rejected;
    }
    
}