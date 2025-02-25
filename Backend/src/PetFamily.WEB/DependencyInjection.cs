using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Core.Caching;
using PetFamily.Core.Options;
using PetFamily.Framework.Authorization;

namespace PetFamily.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.ConsentCookie.IsEssential = true;
            options.CheckConsentNeeded = context => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.HttpOnly  = HttpOnlyPolicy.None;
            options.Secure = CookieSecurePolicy.None;
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing JWT configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.Create(jwtOptions);
            });
        
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        return services;
    }

    public static IServiceCollection AddDistributedCache(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            string connection = configuration.GetConnectionString("Redis")
                                      ?? throw new ArgumentNullException(nameof(connection));
            options.Configuration = connection;
        });

        services.AddSingleton<ICacheService, DistributedCacheService>();
        
        return services;
    }
}