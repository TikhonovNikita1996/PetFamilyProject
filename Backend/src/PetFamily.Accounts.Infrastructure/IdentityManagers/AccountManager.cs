using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AccountManager(WriteAccountsDbContext writeAccountsDbContext)
{
    public async Task<UnitResult<CustomErrorsList>> CreateAdminAccount(AdminAccount adminAccount)
    {
        try
        {
            await writeAccountsDbContext.AdminAccounts.AddAsync(adminAccount);
            await writeAccountsDbContext.SaveChangesAsync();
            return UnitResult.Success<CustomErrorsList>();
        }
        catch (Exception e)
        {
            return CustomError.Failure("could.not.create.admin_account", e.Message).ToErrorList();
        }
    }
}