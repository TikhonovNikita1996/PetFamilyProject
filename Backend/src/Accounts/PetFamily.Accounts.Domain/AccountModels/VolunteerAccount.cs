using Pet.Family.SharedKernel.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain.AccountModels;

public class VolunteerAccount
{
    public VolunteerAccount(){}
    public VolunteerAccount(User user, WorkingExperience experience)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
        WorkingExperience = experience;
    }
    public const string RoleName = "Volunteer";
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public WorkingExperience WorkingExperience { get; private set; } = default!;
}