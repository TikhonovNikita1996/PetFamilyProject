using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Database;
using PetFamily.Core.Dtos.Account;

namespace PetFamily.Accounts.Infrastructure.DbContexts;

public class ReadAccountsDbContext(string ConnectionString) : DbContext, IAccountsReadDbContext
{
    public IQueryable<UserDto> Users => Set<UserDto>();
    public IQueryable<AdminAccountDto> AdminAccounts => Set<AdminAccountDto>();
    public IQueryable<VolunteerAccountDto> VolunteerAccounts => Set<VolunteerAccountDto>();
    public IQueryable<ParticipantAccountDto> ParticipantAccounts => Set<ParticipantAccountDto>();
    
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
            typeof(ReadAccountsDbContext).Assembly,
            x => x.FullName!.Contains("Configurations.Read"));
        modelBuilder.HasDefaultSchema("PetFamily_Accounts");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}