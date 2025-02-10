using System.Security.Authentication;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain;
using PetFamily.Framework.Authorization;

namespace PetFamily.API.Middlewares;

public class UserScopedDataMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserScopedDataMiddleware> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserScopedDataMiddleware(
        RequestDelegate next,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<UserScopedDataMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context, UserScopedData userScopedData)
    {
        if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
        {
            string userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id)!.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new AuthenticationException("The user id claim is not in a valid format.");

            if (userScopedData.UserId == userId)
            {
                await _next(context);
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();

            var accountContract = scope.ServiceProvider.GetRequiredService<IAccountContracts>();
            
            userScopedData.UserId = userId;
            
            var roles = await accountContract.GetUserRoles(userId);
            
            userScopedData.Roles = roles.Select(r => r.Name).ToList()!;
            
            var permissions = await accountContract.GetUserPermissionCodes(userId);
            
            userScopedData.Permissions = permissions.ToList();
            
            _logger.LogInformation("Roles and permission sets to user scoped data");
        }

        await _next(context);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseScopeDataMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserScopedDataMiddleware>();
    }
}