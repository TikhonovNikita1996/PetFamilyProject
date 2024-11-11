namespace PetFamily.Domain.Entities;

public record SocialMedia
{
    public string Name { get; private set; } = default!;
    public string Url { get; private set; } = default!;
}
