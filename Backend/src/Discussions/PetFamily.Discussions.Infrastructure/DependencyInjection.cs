using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Interfaces;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Infrastructure.Consumers;
using PetFamily.Discussions.Infrastructure.DataContexts;
using PetFamily.Discussions.Infrastructure.Repositories;

namespace PetFamily.Discussions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration)
            .AddRepositories()
            .AddUnitOfWork()
            .AddMessageBus(configuration);
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ProjectConstants.Context.Discussions);
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<DiscussionsWriteDbContext>(_ => 
            new DiscussionsWriteDbContext(configuration.GetConnectionString("Database")!));
        
        services.AddScoped<IDiscussionsReadDbContext, DiscussionsReadDbContext>(_ => 
            new DiscussionsReadDbContext(configuration.GetConnectionString("Database")!));
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // services.AddScoped<IRelationRepository, RelationRepository>();
        services.AddScoped<IDiscussionRepository, DiscussionRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit<IDiscussionMessageBus>(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<CreateDiscussionConsumer>();
            
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