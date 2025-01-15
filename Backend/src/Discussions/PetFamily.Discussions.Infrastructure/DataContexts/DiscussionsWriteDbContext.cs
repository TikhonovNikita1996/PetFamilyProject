using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Infrastructure.DataContexts;

public class DiscussionsWriteDbContext : DbContext
{
    private readonly string _connectionString;

    public DiscussionsWriteDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    DbSet<Discussion> Discussions => Set<Discussion>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(DiscussionsWriteDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Write"));
        modelBuilder.HasDefaultSchema("PetFamily_Discussions");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}