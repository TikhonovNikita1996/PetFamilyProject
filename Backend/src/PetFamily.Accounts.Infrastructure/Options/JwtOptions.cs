namespace PetFamily.Accounts.Infrastructure.Options;

public class JwtOptions
{
    public const string JWT = nameof(JWT);
    
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public string Key { get; init; }
    
    public string ExpirationTime { get; init; }
}