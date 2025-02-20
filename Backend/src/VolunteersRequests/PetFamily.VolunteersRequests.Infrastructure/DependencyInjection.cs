using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteersRequests.Application.Database;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Infrastructure.DataContexts;
using PetFamily.VolunteersRequests.Infrastructure.Outbox;

namespace PetFamily.VolunteersRequests.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersRequestsInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration)
            .AddUnitOfWork()
            .AddRepositories()
            .AddOutbox()
            .AddMessageBus(configuration);
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ProjectConstants.Context.VolunteersRequest);
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<VolunteersRequestWriteDbContext>(_ => 
            new VolunteersRequestWriteDbContext(configuration.GetConnectionString("Database")!));
        
        services.AddScoped<IVolunteersRequestReadDbContext, VolunteersRequestReadDbContext>(_ => 
            new VolunteersRequestReadDbContext(configuration.GetConnectionString("Database")!));

        return services;
    }
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRequestRepository, VolunteersRequestsRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        return services;
    }
    
    private static IServiceCollection AddOutbox(
        this IServiceCollection services)
    {
        services.AddScoped<ProcessOutboxMessagesService>();
        return services;
    }
    
    private static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit<IVolunteerRequestMessageBus>(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();
            
            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMQ:Host"]!), h =>
                {
                    h.Username(configuration["RabbitMQ:UserName"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}