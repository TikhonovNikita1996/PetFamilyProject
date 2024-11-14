using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record SocialMedia
{
    private SocialMedia(string name, string url)
    {
        Name = name;
        Url = url;
    }
    public string Name { get; } = default!;
    public string Url { get; } = default!;
    public static CustomResult<SocialMedia> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name can not be empty";
        if (string.IsNullOrWhiteSpace(url))
            return "Link can not be empty";

        var newSocialNetwork = new SocialMedia(name, url);

        return newSocialNetwork;
    }
}
