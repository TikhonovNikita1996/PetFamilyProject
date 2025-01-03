using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Interfaces;

public interface IRefreshSessionManager
{
    public Task<Result<RefreshSession, CustomError>> GetByRefreshToken(Guid refreshToken,
        CancellationToken cancellationToken);

    public void Delete(RefreshSession refreshSession);
}