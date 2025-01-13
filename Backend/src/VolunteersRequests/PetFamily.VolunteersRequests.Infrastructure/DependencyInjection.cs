using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteersRequests.Infrastructure.DataContexts;

namespace PetFamily.VolunteersRequests.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersRequestsInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration)
                .AddUnitOfWork();
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ProjectConstants.Context.SpeciesManagement);
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<VolunteersRequestWriteDbContext>(_ => 
            new VolunteersRequestWriteDbContext(configuration.GetConnectionString("Database")!));

        return services;
    }
}