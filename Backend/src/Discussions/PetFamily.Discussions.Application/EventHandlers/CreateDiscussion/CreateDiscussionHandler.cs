using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Events.VolunteerRequest;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.EventHandlers.CreateDiscussion;

public class CreateDiscussionHandler : INotificationHandler<CreateDiscussionEvent>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IDiscussionsReadDbContext _discussionsReadDbContext;

    public CreateDiscussionHandler(IDiscussionRepository discussionRepository,
        IDiscussionsReadDbContext discussionsReadDbContext)
    {
        _discussionRepository = discussionRepository;
        _discussionsReadDbContext = discussionsReadDbContext;
    }
    
    public async Task Handle(CreateDiscussionEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var discussionUsers = DiscussionUsers.Create(domainEvent.ReviewingUserId, domainEvent.ApplicantUserId);
        
        var discussion = Discussion.Create(discussionUsers);

        if (discussion.IsFailure)
            throw new Exception(discussion.Error.Message);

        await _discussionRepository.Add(discussion.Value, cancellationToken);
    }
}