using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Species.Application.Database;

namespace PetFamily.Species.Infrastructure.DataContexts;

public class SpeciesReadDbContext(string ConnectionString) : DbContext, IReadDbContext
{
    public IQueryable<SpecieDto> Species => Set<SpecieDto>();
    public IQueryable<BreedDto> Breeds => Set<BreedDto>();
    
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
            typeof(SpeciesReadDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));
        modelBuilder.HasDefaultSchema("PetFamily_Species");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}