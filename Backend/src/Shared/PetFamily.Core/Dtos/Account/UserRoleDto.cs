namespace PetFamily.Core.Dtos.Account;

public class UserRoleDto
{
    public Guid RoleId { get; init; }
    public RoleDto Role { get; init; } = default!;

    public Guid UserId { get; init; }
    public RoleDto User { get; init; } = default!;
}