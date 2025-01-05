namespace PetFamily.Core.Dtos.Account;

public class RolePermissionDto
{
    public Guid UserId { get; init; }
    public UserDto User { get; init; } = default!;
    public Guid RoleId { get; init; }
    public RoleDto Role { get; init; } = default!;
}