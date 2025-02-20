using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Core.Events.VolunteerRequest;
using PetFamily.VolunteersRequests.Contracts.Messages;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain;

public class VolunteerRequest : DomainEntity<VolunteerRequestId>
{
    // ef core
    public VolunteerRequest(VolunteerRequestId id) : base(id) {}
    public Guid? AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public Guid? DiscussionId { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public RejectionComment? RejectionComment { get; private set; }
    
    private VolunteerRequest(
        VolunteerRequestId requestId,
        Guid userId,
        VolunteerInfo volunteerInfo) : base(requestId)
    {
        UserId = userId;
        Id = requestId;
        CreatedAt = DateTime.UtcNow;
        Status = RequestStatus.Submitted;
        VolunteerInfo = volunteerInfo;
    }
    
    public static Result<VolunteerRequest, CustomError>  Create(
        VolunteerRequestId requestId,
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        var request = new VolunteerRequest(requestId, userId, volunteerInfo);
        return request;
    }
    
    public void TakeInReview(Guid adminId)
    {
        AdminId = adminId;
        Status = RequestStatus.OnReview;

        AddDomainEvent(new VolunteerRequestReviewStartedEvent(adminId, Id.Value));
    }
    
    public void SetRevisionRequiredStatus(
        RejectionComment rejectedComment)
    {
        Status = RequestStatus.RevisionRequired;
        RejectionComment = rejectedComment;
    }

    public void SetApprovedStatus()
    {
        Status = RequestStatus.Approved;
        AddDomainEvent(new VolunteerRequestApprovedEvent(UserId));
    }
    
    public void SetRejectStatus(RejectionComment rejectedComment)
    {
        RejectionComment = rejectedComment;
        Status = RequestStatus.Rejected;
    }
    
    public void Refresh()
    {
        Status = RequestStatus.Submitted;
    }
    
}