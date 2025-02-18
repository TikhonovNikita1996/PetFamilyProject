﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Core.Events.VolunteerRequest;

namespace PetFamily.Accounts.Application.AccountsManagement.EventHandlers.CreateVolunteerAccountForUser;

public class CreateVolunteerAccountForUser : INotificationHandler<CreateVolunteerAccountEvent>
{
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public CreateVolunteerAccountForUser(
        IAccountManager accountManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _accountManager = accountManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task Handle(CreateVolunteerAccountEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(domainEvent.UserId.ToString());
        if (user == null)
            throw new Exception("User not found");
        
        var workingExperience = WorkingExperience.Create(0).Value;
        
        var volunteerRole = await _roleManager.FindByNameAsync(VolunteerAccount.RoleName)
                            ?? throw new ApplicationException("Volunteer role is not found");
        
        var volunteerAccount = new VolunteerAccount(user!, workingExperience);
        
        user!.VolunteerAccount = volunteerAccount;
        
        user.ChangeRole(volunteerRole);
        
        var result = await _accountManager.CreateVolunteerAccount(volunteerAccount);
        
        if (result.IsFailure)
            throw new Exception("Smth went wrong");
    }
}