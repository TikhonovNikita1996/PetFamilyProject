namespace PetFamily.Accounts.Contracts;

public interface IAccountContracts
{
    public Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
}