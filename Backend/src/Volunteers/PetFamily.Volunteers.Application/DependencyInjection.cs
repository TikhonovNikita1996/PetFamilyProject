using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddValidatorsFromAssembly(assembly);   
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(c => c.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces().WithScopedLifetime());
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces().WithScopedLifetime());
        
        return services;
    }

}