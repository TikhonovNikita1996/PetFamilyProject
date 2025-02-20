using MassTransit;
using PetFamily.Core.Events.VolunteerRequest;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;
using PetFamily.VolunteersRequests.Contracts.Messages;

namespace PetFamily.Discussions.Infrastructure.Consumers;

public class CreateDiscussionConsumer : IConsumer<CreateDiscussionEvent>
{
    private readonly IDiscussionRepository _discussionRepository;

    public CreateDiscussionConsumer(IDiscussionRepository discussionRepository)
    {
        _discussionRepository = discussionRepository;
    }
    public async Task Consume(ConsumeContext<CreateDiscussionEvent> context)
    {
        var discussionUsers = DiscussionUsers.Create(context.Message.ReviewingUserId,
            context.Message.ApplicantUserId);
        
        var discussion = Discussion.Create(discussionUsers);

        if (discussion.IsFailure)
            throw new Exception(discussion.Error.Message);

        await _discussionRepository.Add(discussion.Value, new CancellationToken());
    }
}