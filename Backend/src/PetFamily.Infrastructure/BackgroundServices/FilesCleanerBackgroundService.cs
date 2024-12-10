using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Minio;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundService(ILogger<FilesCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FileMetaData>> messageQueue, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is starting.");
        await using var scope = _scopeFactory.CreateAsyncScope();

        var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFileCleanerService>();
        
        while (!cancellationToken.IsCancellationRequested)
        {
            await filesCleanerService.Process(cancellationToken);
        }
        await Task.CompletedTask;
    }

}