using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain;

namespace PetFamily.Infrastructure;

public class DataContext(IConfiguration configuration) : DbContext
{
    public DbSet<Pet> Pets { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DataBase"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}