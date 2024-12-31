using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class RolePermissionManager(WriteAccountsDbContext writeAccountsDbContext)
{
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissionCodes)
    {
        foreach (var permissionCode in permissionCodes)
        {
            var permission = await writeAccountsDbContext.Permissions
                .FirstOrDefaultAsync(permission => permission.Code == permissionCode);
            
            var rolePermissionxist = await writeAccountsDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);
            
            if(rolePermissionxist)
                continue;

            writeAccountsDbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });

        }
        await writeAccountsDbContext.SaveChangesAsync();
    }
}