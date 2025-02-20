using MassTransit;
using MediatR;
using PetFamily.Core.Events.VolunteerRequest;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Contracts.Messages;

namespace PetFamily.VolunteersRequests.Application.EventHandlers;

public class SendDiscussionCreationEvent : INotificationHandler<VolunteerRequestReviewStartedEvent>
{
    private readonly IOutboxRepository _outboxRepository;

    public SendDiscussionCreationEvent(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }
    
    public async Task Handle(VolunteerRequestReviewStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new CreateDiscussionEvent(
            domainEvent.ReviewingUserId,
            domainEvent.ApplicantUserId);

        await _outboxRepository.Add(
            integrationEvent,
            cancellationToken);
    }
    
}