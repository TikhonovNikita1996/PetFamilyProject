using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Contracts;

public interface IAccountContracts
{
    public Task<HashSet<string>> GetUserPermissionCodes(Guid userId);

    public Task<HashSet<Role>> GetUserRoles (Guid userId);
    public Task<bool> IsUserBannedForVolunteerRequests(Guid userId, CancellationToken cancellationToken);
    public Task BanUser(Guid userId, CancellationToken cancellationToken);
}