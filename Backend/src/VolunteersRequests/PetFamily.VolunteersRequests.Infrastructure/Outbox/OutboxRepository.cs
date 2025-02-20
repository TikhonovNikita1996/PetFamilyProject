using System.Text.Json;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Infrastructure.DataContexts;

namespace PetFamily.VolunteersRequests.Infrastructure.Outbox;

public class OutboxRepository : IOutboxRepository
{
    private readonly VolunteersRequestWriteDbContext _dbContext;

    public OutboxRepository(VolunteersRequestWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add<T>(T message, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.Now,
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message),
        };

        await _dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}