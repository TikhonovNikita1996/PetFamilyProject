using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AccountManager(WriteAccountsDbContext writeAccountsDbContext) : IAccountManager
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
    
    public async Task<UnitResult<CustomErrorsList>> CreateParticipantAccount(ParticipantAccount adminAccount)
    {
        try
        {
            await writeAccountsDbContext.ParticipantAccounts.AddAsync(adminAccount);
            await writeAccountsDbContext.SaveChangesAsync();
            return UnitResult.Success<CustomErrorsList>();
        }
        catch (Exception e)
        {
            return CustomError.Failure("could.not.create.participant_account", e.Message).ToErrorList();
        }
    }
    
    public async Task<UnitResult<CustomErrorsList>> CreateVolunteerAccount(VolunteerAccount adminAccount)
    {
        try
        {
            await writeAccountsDbContext.VolunteerAccounts.AddAsync(adminAccount);
            await writeAccountsDbContext.SaveChangesAsync();
            return UnitResult.Success<CustomErrorsList>();
        }
        catch (Exception e)
        {
            return CustomError.Failure("could.not.create.volunteer_account", e.Message).ToErrorList();
        }
    }
}