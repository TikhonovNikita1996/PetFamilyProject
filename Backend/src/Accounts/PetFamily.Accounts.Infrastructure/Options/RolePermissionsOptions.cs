namespace PetFamily.Accounts.Infrastructure.Options;

public class RolePermissionsOptions
{
    public Dictionary<string, string[]> Roles { get; set; } = [];
    public Dictionary<string, string[]> Permissions { get; set; } = [];
}