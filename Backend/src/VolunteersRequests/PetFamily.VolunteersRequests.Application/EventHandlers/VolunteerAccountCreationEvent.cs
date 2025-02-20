using MediatR;
using PetFamily.Core.Events.VolunteerRequest;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Contracts.Messages;

namespace PetFamily.VolunteersRequests.Application.EventHandlers;

public class VolunteerAccountCreationEvent : INotificationHandler<VolunteerRequestApprovedEvent>
{
    private readonly IOutboxRepository _outboxRepository;

    public VolunteerAccountCreationEvent(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }
    
    public async Task Handle(VolunteerRequestApprovedEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new CreateVolunteerAccountEvent(
            domainEvent.UserId);

        await _outboxRepository.Add(
            integrationEvent,
            cancellationToken);
    }
}