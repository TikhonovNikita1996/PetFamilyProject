using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Accounts.Infrastructure.IdentityManagers;

namespace PetFamily.Accounts.Presentation;

public class AccountContracts : IAccountContracts
{
    private readonly PermissionManager _permissionManager;
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AccountContracts(PermissionManager permissionManager
        ,IAccountManager accountManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _permissionManager = permissionManager;
        _accountManager = accountManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        return await _permissionManager.GetUserPermissionsCode(userId);
    }

    public async Task<Result<Guid, CustomErrorsList>> CreateVolunteerAccountForUser(Guid userid,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userid.ToString());
        var workingExperience = WorkingExperience.Create(0).Value;
        
        var volunteerRole = await _roleManager.FindByNameAsync(VolunteerAccount.RoleName)
                              ?? throw new ApplicationException("Volunteer role is not found");
        
        var volunteerAccount = new VolunteerAccount(user!, workingExperience);
        
        user.VolunteerAccount = volunteerAccount;
        
        user.ChangeRole(volunteerRole);
        
        var result = await _accountManager.CreateVolunteerAccount(volunteerAccount);
        
        if (result.IsFailure)
            return result.Error;
        
        return volunteerAccount.Id;
    }
}