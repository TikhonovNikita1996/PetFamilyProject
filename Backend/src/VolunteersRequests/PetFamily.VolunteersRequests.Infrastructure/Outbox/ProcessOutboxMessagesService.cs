using System.Text.Json;
using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Discussions.Application.Interfaces;
using PetFamily.VolunteersRequests.Application;
using PetFamily.VolunteersRequests.Infrastructure.DataContexts;
using Polly;
using Polly.Retry;

namespace PetFamily.VolunteersRequests.Infrastructure.Outbox;

public class ProcessOutboxMessagesService
{
    private readonly VolunteersRequestWriteDbContext _dbContext;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<ProcessOutboxMessagesService> _logger;

    public ProcessOutboxMessagesService(
        Bind<IDiscussionMessageBus, IPublishEndpoint> publisher,
        VolunteersRequestWriteDbContext dbContext,
        ILogger<ProcessOutboxMessagesService> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher.Value;
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        var messages = await _dbContext
            .Set<OutboxMessage>()
            .OrderBy(m => m.OccurredOnUtc)
            .Where(m => m.ProcessedOnUtc == null)
            .Take(50)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
            return;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(2),
                ShouldHandle = new PredicateBuilder().Handle<Exception>(e => e is not NullReferenceException),
                OnRetry = retryArguments =>
                {
                    _logger.LogCritical(
                        retryArguments.Outcome.Exception,
                        "Current attempt: {attemptNumber}",
                        retryArguments.AttemptNumber);

                    return ValueTask.CompletedTask;
                },
            })
            .Build();

        var processingTasks = messages.Select(message => ProcessMessageAsync(message, pipeline, cancellationToken));
        await Task.WhenAll(processingTasks);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save changes to the database.");
        }
    }

    private async Task ProcessMessageAsync(
        OutboxMessage message,
        ResiliencePipeline pipeline,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var messageType = AssemblyReference.Assembly.GetType(message.Type)
                              ?? throw new NullReferenceException("Message type not found");

            var deserializedMessage = JsonSerializer.Deserialize(message.Payload, messageType)
                                      ?? throw new NullReferenceException("Message payload not found");

            await pipeline.ExecuteAsync(
                async token =>
            {
                await _publisher.Publish(deserializedMessage, messageType, token);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            message.Error = ex.Message;
            message.ProcessedOnUtc = DateTime.UtcNow;
            _logger.LogError(ex, "Failed to process message ID: {MessageId}", message.Id);
        }
    }
}