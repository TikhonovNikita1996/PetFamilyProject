using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountContracts, AccountContracts>();
        services.AddHttpContextAccessor()
            .AddScoped<UserScopedData>();
        return services;
    }
}