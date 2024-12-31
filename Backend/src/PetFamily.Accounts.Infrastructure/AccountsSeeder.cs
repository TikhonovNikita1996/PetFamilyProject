using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AccountsSeeder> _logger;

    public AccountsSeeder(IServiceScopeFactory serviceScopeFactory,
        ILogger<AccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var json = await File.ReadAllTextAsync(JsonPaths.AccountsSeeder);
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();
        var rolePermissionManager = scope.ServiceProvider.GetRequiredService<RolePermissionManager>();
        var seedData = JsonSerializer.Deserialize<RolePermissionsConfig>(json)
            ?? throw new ApplicationException("Could not deserialize Role permissions config");

        await SeedPermissions(seedData, permissionManager);
        await SeedRoles(seedData, roleManager);
        await SeedRolePermissions(seedData, roleManager, rolePermissionManager);
    }

    private async Task SeedRolePermissions(RolePermissionsConfig seedData, RoleManager<Role> roleManager,
        RolePermissionManager rolePermissionManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var rolePermissions = seedData.Roles[roleName];
            await rolePermissionManager.AddRangeIfExist(role!.Id, rolePermissions);
        }
    }

    private async Task SeedRoles(RolePermissionsConfig seedData, RoleManager<Role> roleManager)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var existingRole = await roleManager.FindByNameAsync(roleName);
            if (existingRole == null)
                await roleManager.CreateAsync(new Role {Name = roleName});
        }
    }

    private async Task SeedPermissions(RolePermissionsConfig seedData, PermissionManager permissionManager)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);

        await permissionManager.AddRangeIfExist(permissionsToAdd);
    }
}

public class RolePermissionsConfig
{
    public Dictionary<string, string[]> Roles { get; set; } = [];
    public Dictionary<string, string[]> Permissions { get; set; } = [];
}