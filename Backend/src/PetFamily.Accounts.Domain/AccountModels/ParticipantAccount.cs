namespace PetFamily.Accounts.Domain.AccountModels;

public class ParticipantAccount
{
    public const string RoleName = "Participant";
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}