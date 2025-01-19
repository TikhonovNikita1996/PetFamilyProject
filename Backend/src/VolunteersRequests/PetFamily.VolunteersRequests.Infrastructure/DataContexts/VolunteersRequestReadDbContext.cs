using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Core.Dtos.VolunteersRequest;
using PetFamily.VolunteersRequests.Application.Database;

namespace PetFamily.VolunteersRequests.Infrastructure.DataContexts;

public class VolunteersRequestReadDbContext(string ConnectionString) : DbContext, IVolunteersRequestReadDbContext
{
    public IQueryable<VolunteersRequestDto> VolunteersRequests => Set<VolunteersRequestDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(VolunteersRequestReadDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));
        modelBuilder.HasDefaultSchema("PetFamily_VolunteersRequests");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}