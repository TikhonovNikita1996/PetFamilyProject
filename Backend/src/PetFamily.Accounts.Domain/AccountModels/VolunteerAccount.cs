using Pet.Family.SharedKernel.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain.AccountModels;

public class VolunteerAccount
{
    public const string RoleName = "Volunteer";
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public WorkingExperience WorkingExperience { get; private set; } = default!;
}