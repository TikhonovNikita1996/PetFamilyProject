using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure.DataSeeding;

public class AccountsSeederService (
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    IOptions<AdminOptions> adminOptions,
    IAccountManager accountManager,
    ILogger<AccountsSeederService> logger)
{
    private readonly AdminOptions _adminOptions = adminOptions.Value;

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding accounts...");
        
        var json = await File.ReadAllTextAsync(JsonPaths.AccountsSeeder);
        
        var seedData = JsonSerializer.Deserialize<RolePermissionsOptions>(json)
                       ?? throw new ApplicationException("Could not deserialize Role permissions config");

        await SeedPermissions(seedData);
        await SeedRoles(seedData);
        await SeedRolePermissions(seedData);
        await SeedAdmin();
    }
    
    private async Task SeedRolePermissions(RolePermissionsOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var rolePermissions = seedData.Roles[roleName];
            await rolePermissionManager.AddRangeIfExist(role!.Id, rolePermissions);
        }
    }

    private async Task SeedRoles(RolePermissionsOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var existingRole = await roleManager.FindByNameAsync(roleName);
            if (existingRole == null)
                await roleManager.CreateAsync(new Role {Name = roleName});
        }
    }

    private async Task SeedPermissions(RolePermissionsOptions seedData)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);

        await permissionManager.AddRangeIfExist(permissionsToAdd);
    } 
    
    private async Task SeedAdmin()
    {
        var isAdminExist = await userManager.FindByNameAsync(_adminOptions.UserName);

        if (isAdminExist != null)
            return;
        
        var adminRole = await roleManager.FindByNameAsync(AdminAccount.RoleName)
                        ?? throw new ApplicationException("Admin role is not found");

        var adminName = FullName
            .Create(_adminOptions.UserName, _adminOptions.UserName, _adminOptions.UserName).Value;
        
        var adminUser = User.CreateAdmin(_adminOptions.Email, _adminOptions.UserName, adminName, adminRole);
        await userManager.CreateAsync(adminUser, _adminOptions.Password);
        
        var adminAccount = new AdminAccount(adminUser);
        await accountManager.CreateAdminAccount(adminAccount);
        
        adminUser.AdminAccount = adminAccount;
        
        await userManager.UpdateAsync(adminUser);
    }
}

