using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DataSeeding;
using PetFamily.Accounts.Infrastructure.DbContexts;
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
        services
            .AddDbContexts(configuration)
            .AddManagers()
            .AddInfrastructureIdentity(configuration)
            .AddAccountsSeeding()
            .AddUnitOfWork();
        
        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ProjectConstants.Context.AccountManagement);
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services
        ,IConfiguration configuration)
    {
        services.AddScoped<WriteAccountsDbContext>(_ => 
            new WriteAccountsDbContext(configuration.GetConnectionString("Database")!));
        
        services.AddScoped<IAccountsReadDbContext, ReadAccountsDbContext>(_ => 
            new ReadAccountsDbContext(configuration.GetConnectionString("Database")!));
        
        return services;
    }
    
    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<IAccountManager, AccountManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        
        return services;
    }
    
    private static IServiceCollection AddInfrastructureIdentity(this IServiceCollection services
        , IConfiguration configuration)
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
        
        return services;
    }
    
    private static IServiceCollection AddAccountsSeeding(this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        
        return services;
    }
    
}