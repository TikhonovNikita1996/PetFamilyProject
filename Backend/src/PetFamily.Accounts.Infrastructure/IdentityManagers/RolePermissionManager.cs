using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager(WriteAccountsDbContext writeAccountsDbContext)
{
    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissionCodes,
        CancellationToken cancelToken = default)
    {
        foreach (var permissionCode in permissionCodes)
        {
            var permission = await writeAccountsDbContext.Permissions
                .FirstOrDefaultAsync(permission => permission.Code == permissionCode,cancelToken);
            if(permission == null)
                throw new ApplicationException($"Permission code {permissionCode} not found");
            
            var rolePermissionExist = await writeAccountsDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id, cancelToken);
            
            if(rolePermissionExist)
                continue;

            writeAccountsDbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission!.Id
            });

        }
        await writeAccountsDbContext.SaveChangesAsync(cancelToken);
    }
}