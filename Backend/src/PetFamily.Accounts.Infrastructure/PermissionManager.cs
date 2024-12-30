using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class PermissionManager(WriteAccountsDbContext writeAccountsDbContext)
{
        
    public async Task<Permission?> FindByCode(string permissionCode, CancellationToken stoppingToken = default)
    {
        var permission = await writeAccountsDbContext.Permissions
            .FirstOrDefaultAsync(permission => permission.Code == permissionCode, stoppingToken);
        
        return permission;
    }
    
    public async Task AddRangeIfExist(IEnumerable<string> permissionCodes)
    {
        foreach (var permissionCode in permissionCodes)
        {
            var isPermissionExist = await writeAccountsDbContext.Permissions
                .AnyAsync(p => p.Code == permissionCode);
            
            if(isPermissionExist)
                return;
            
            await writeAccountsDbContext.Permissions.AddAsync(new Permission {Code = permissionCode});
        }
        await writeAccountsDbContext.SaveChangesAsync();
    }
}