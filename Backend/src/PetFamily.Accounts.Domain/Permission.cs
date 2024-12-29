namespace PetFamily.Accounts.Domain;

public class Permission
{
    public Permission(Guid id, string code)
    {
        Id = id;
        Code = code;
    }
    public Guid Id { get; set; }
    public string Code { get; set; }
}