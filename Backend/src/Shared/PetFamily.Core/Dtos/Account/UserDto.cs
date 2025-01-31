using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Core.Dtos.Account;

public class UserDto
{
    public Guid Id { get; init; }
    
    public string LastName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? MiddleName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserAvatarDto? Photo { get; init; }
    public AdminAccountDto? AdminAccount { get; init; }
    public VolunteerAccountDto? VolunteerAccount { get; init; }
    public ParticipantAccountDto? ParticipantAccount { get; init; } = null!;

}