using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Core.Dtos.Account;

public class VolunteerAccountDto
{
    public Guid VolunteerAccountId { get; init; }
    public Guid UserId { get; init; }
    public DateTime BannedForRequestsUntil { get; set; }
    public int WorkingExperience { get; init; } = default!;
}