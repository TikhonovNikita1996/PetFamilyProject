using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Infrastructure;

namespace PetFamily.Discussions.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionsInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        // services.AddScoped<IRelationContracts, RelationContracts>();
        // services.AddScoped<IDiscussionContracts, DiscussionContracts>();
        return services;
    }
}