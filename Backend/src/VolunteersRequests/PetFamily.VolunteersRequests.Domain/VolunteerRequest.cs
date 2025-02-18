﻿using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain;

public class VolunteerRequest : DomainEntity<VolunteerRequestId>
{
    public VolunteerRequestId RequestId { get; set; }
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
        RequestId = requestId;
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
    
    public void TakeInReview(Guid adminId, Guid discussionId)
    {
        AdminId = adminId;
        DiscussionId = discussionId;
        Status = RequestStatus.OnReview;
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