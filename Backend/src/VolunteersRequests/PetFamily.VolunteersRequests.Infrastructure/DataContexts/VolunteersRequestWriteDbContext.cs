﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Infrastructure.Outbox;

namespace PetFamily.VolunteersRequests.Infrastructure.DataContexts;

public class VolunteersRequestWriteDbContext : DbContext
{
    private readonly string _connectionString;

    public VolunteersRequestWriteDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbSet<VolunteerRequest> VolunteersRequests => Set<VolunteerRequest>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    
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
            typeof(VolunteersRequestWriteDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Write"));
        modelBuilder.HasDefaultSchema("PetFamily_VolunteersRequests");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}