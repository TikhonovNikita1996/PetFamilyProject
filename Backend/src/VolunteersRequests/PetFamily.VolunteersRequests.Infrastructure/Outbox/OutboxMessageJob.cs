using Quartz;

namespace PetFamily.VolunteersRequests.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ProcessOutboxMessagesService _outboxMessagesService;

    public ProcessOutboxMessagesJob(ProcessOutboxMessagesService outboxMessagesService)
    {
        _outboxMessagesService = outboxMessagesService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _outboxMessagesService.Execute(context.CancellationToken);
    }
}