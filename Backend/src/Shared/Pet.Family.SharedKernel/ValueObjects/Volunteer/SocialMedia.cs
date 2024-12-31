using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Volunteer;

public record SocialMedia
{
    private SocialMedia(string name, string url)
    {
        Name = name;
        Url = url;
    }
    public string Name { get; } = default!;
    public string Url { get; } = default!;
    public static Result<SocialMedia, CustomError> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(name);
        if (string.IsNullOrWhiteSpace(url))
            return Errors.General.ValueIsInvalid(name);

        var newSocialNetwork = new SocialMedia(name, url);

        return newSocialNetwork;
    }
}
