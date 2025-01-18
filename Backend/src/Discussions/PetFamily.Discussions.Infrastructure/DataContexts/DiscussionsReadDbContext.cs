using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Infrastructure.DataContexts;

public class DiscussionsReadDbContext(string ConnectionString) : DbContext, IDiscussionsReadDbContext
{
    public IQueryable<RelationDto> Relations => Set<RelationDto>();
    public IQueryable<DiscussionDto> Discussions => Set<DiscussionDto>();
    
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
            typeof(DiscussionsReadDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));
        modelBuilder.HasDefaultSchema("PetFamily_Species");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
    
}