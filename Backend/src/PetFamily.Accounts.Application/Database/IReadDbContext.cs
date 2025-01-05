using PetFamily.Core.Dtos.Account;

namespace PetFamily.Accounts.Application.Database;

public interface IReadDbContext
{
    public IQueryable<UserDto> Users { get; }
    public IQueryable<AdminAccountDto> AdminAccounts { get; }
    public IQueryable<VolunteerAccountDto> VolunteerAccounts { get; }
    public IQueryable<ParticipantAccountDto> ParticipantAccounts { get; }
    public IQueryable<RoleDto> Roles { get; }
}