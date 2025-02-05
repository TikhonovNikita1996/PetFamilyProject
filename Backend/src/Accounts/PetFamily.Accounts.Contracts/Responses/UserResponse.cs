using PetFamily.Core.Dtos.Account;

namespace PetFamily.Accounts.Contracts.Responses;

public class UserResponse
{
    public Guid Id { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? MiddleName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid PhotoId { get; init; }
    public string PhotoUrl { get; init; } = string.Empty;
    public AdminAccountDto? AdminAccount { get; init; }
    public VolunteerAccountDto? VolunteerAccount { get; init; }
    public ParticipantAccountDto? ParticipantAccount { get; init; } = null!;
}