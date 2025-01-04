using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager(WriteAccountsDbContext writeAccountsDbContext)
{
        
    public async Task<Permission?> FindByCode(string permissionCode, CancellationToken cancellationToken = default)
    {
        var permission = await writeAccountsDbContext.Permissions
            .FirstOrDefaultAsync(permission => permission.Code == permissionCode, cancellationToken);
        
        return permission;
    }
    
    public async Task AddRangeIfExist(IEnumerable<string> permissionCodes,
        CancellationToken cancellationToken = default)
    {
        foreach (var permissionCode in permissionCodes)
        {
            var isPermissionExist = await writeAccountsDbContext.Permissions
                .AnyAsync(p => p.Code == permissionCode, cancellationToken);
            
            if(isPermissionExist)
                return;
            
            await writeAccountsDbContext.Permissions.AddAsync(new Permission {Code = permissionCode}, cancellationToken);
        }
        await writeAccountsDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<HashSet<string>> GetUserPermissionsCode(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var permissions = await writeAccountsDbContext.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync(cancellationToken);
        
        return permissions.ToHashSet();
    }
    
}