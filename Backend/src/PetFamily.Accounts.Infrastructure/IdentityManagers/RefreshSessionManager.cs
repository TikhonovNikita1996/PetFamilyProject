using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager(WriteAccountsDbContext writeAccountsDbContext) : IRefreshSessionManager
{
    public async Task<Result<RefreshSession, CustomError>> GetByRefreshToken(Guid refreshToken,
        CancellationToken cancellationToken)
    {
        var refreshSession =  await writeAccountsDbContext.RefreshSessions
            .Include(r => r.User)
            .FirstOrDefaultAsync(rs => rs.RefreshToken == refreshToken, cancellationToken);
        
        if(refreshSession == null)
            return Errors.General.NotFound("Refresh token");
        
        return refreshSession;
    }
    
    public void Delete (RefreshSession refreshSession)
    {
        writeAccountsDbContext.RefreshSessions.Remove(refreshSession);
    }
}