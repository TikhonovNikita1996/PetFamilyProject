using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Presentation;

public class AccountContracts : IAccountContracts
{
    private readonly PermissionManager _permissionManager;
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountsReadDbContext _accountsReadDbContext;
    private readonly WriteAccountsDbContext _writeAccountsDbContext;
    private readonly IUnitOfWork _unitOfWork;

    public AccountContracts(PermissionManager permissionManager,
        IAccountManager accountManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAccountsReadDbContext accountsReadDbContext,
        WriteAccountsDbContext writeAccountsDbContext,
        [FromKeyedServices(ProjectConstants.Context.AccountManagement)] IUnitOfWork unitOfWork)
    {
        _permissionManager = permissionManager;
        _accountManager = accountManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _accountsReadDbContext = accountsReadDbContext;
        _writeAccountsDbContext = writeAccountsDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        return await _permissionManager.GetUserPermissionsCode(userId);
    }

    public async Task<HashSet<Role>> GetUserRoles(Guid userId)
    {
        return await _permissionManager.GetUserRoles(userId);
    }
    
    public async Task<bool> IsUserBannedForVolunteerRequests(Guid userId, CancellationToken cancellationToken)
    {
        var userDto = await _accountsReadDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        return DateTime.UtcNow < userDto!.BannedForRequestsUntil;
    }

    public async Task BanUser(Guid userId, CancellationToken cancellationToken)
    {
        var participantAccount = await _writeAccountsDbContext.ParticipantAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (participantAccount != null)
            participantAccount.BanForRequestsForWeek(DateTime.UtcNow.AddDays(7));

        await _unitOfWork.SaveChanges(cancellationToken);
    }
}