using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Accounts.Contracts;

public interface IAccountContracts
{
    public Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
    public Task<Result<Guid, CustomErrorsList>> CreateVolunteerAccountForUser(Guid userid,
        CancellationToken cancellationToken);
    public Task<bool> IsUserBannedForVolunteerRequests(Guid userId, CancellationToken cancellationToken);
    public Task BanUser(Guid userId, CancellationToken cancellationToken);
}