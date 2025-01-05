using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Volunteers.Infrastructure.Services;

namespace PetFamily.Volunteers.Infrastructure.BackgroundServices;

public class CleanSoftDeletedEntitiesBackGroundService : BackgroundService
{
    private const int DelayHours = 24;
    
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CleanSoftDeletedEntitiesBackGroundService> _logger;

    public CleanSoftDeletedEntitiesBackGroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<CleanSoftDeletedEntitiesBackGroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeletedEntityCleanupService is starting");
        
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope =  _scopeFactory.CreateAsyncScope();
            
            var deleteExpiredVolunteersService = scope.ServiceProvider
                .GetRequiredService<DeleteExpiredVolunteersService>();
            
            var deleteExpiredPetsService = scope.ServiceProvider
                .GetRequiredService<DeleteExpiredPetsService>();
            
            await deleteExpiredPetsService.Process();
            _logger.LogInformation("Deleted expired pets");
            
            await deleteExpiredVolunteersService.Process(cancellationToken);
            _logger.LogInformation("Deleted expired volunteers");
            
            await Task.Delay(TimeSpan.FromHours(DelayHours), cancellationToken);
        }
    }
}