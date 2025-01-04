using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DataSeeding;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Options;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>();
        
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));
        
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<WriteAccountsDbContext>();
        
        services.AddScoped<WriteAccountsDbContext>(_ => 
            new WriteAccountsDbContext(configuration.GetConnectionString("Database")!));

        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<IAccountManager, AccountManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        services.AddUnitOfWork();
        
        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ProjectConstants.Context.AccountManagement);
        return services;
    }
}