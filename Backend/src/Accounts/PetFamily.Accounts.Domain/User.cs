﻿using Microsoft.AspNetCore.Identity;
using Pet.Family.SharedKernel.ValueObjects;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Domain.AccountModels;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User() {}
    private List<Role> _roles = [];
    private List<SocialMedia> _socialNetworks = [];
    public FullName FullName { get; set; } = null!;
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public UserAvatar? Photo { get; set; }
    public IReadOnlyList<SocialMedia> SocialNetworks => _socialNetworks;
    public AdminAccount? AdminAccount { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    
    public static User CreateAdmin(string email, string userName, FullName fullName, Role role)
    {
        return new User
        {
            Email = email,
            UserName = userName,
            _roles = [role],
            FullName = fullName
        };
    }
    
    public static User CreateParticipant(string email, string userName, FullName fullName, Role role)
    {
        return new User
        {
            Email = email,
            UserName = userName,
            _roles = [role],
            FullName = fullName
        };
    }
    
    public static User CreateVolunteer(string email, string userName, FullName fullName, Role role)
    {
        return new User
        {
            Email = email,
            UserName = userName,
            _roles = [role],
            FullName = fullName
        };
    }

    public void UpdateSocialMediaDetails(List<SocialMedia> socialMedia)
    {
        _socialNetworks = socialMedia;
    }
    
    public void ChangeRole(Role role)
    {
        _roles = [role];
    }
}
