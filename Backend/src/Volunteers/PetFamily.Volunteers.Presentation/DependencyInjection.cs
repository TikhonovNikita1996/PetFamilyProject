using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.Core.Messaging;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Infrastructure;

namespace PetFamily.Volunteers.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddScoped<IVolunteerContracts, VolunteerContracts>();
        return services;
    }
}