using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class RolePermissionManager(AccountsDbContext accountsDbContext)
{
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissionCodes)
    {
        foreach (var permissionCode in permissionCodes)
        {
            var permission = await accountsDbContext.Permissions
                .FirstOrDefaultAsync(permission => permission.Code == permissionCode);
            
            var rolePermissionxist = await accountsDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);
            
            if(rolePermissionxist)
                continue;

            accountsDbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });

        }
        await accountsDbContext.SaveChangesAsync();
    }
}