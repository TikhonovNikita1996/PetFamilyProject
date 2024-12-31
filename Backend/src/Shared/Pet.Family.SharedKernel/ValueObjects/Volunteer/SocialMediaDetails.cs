namespace Pet.Family.SharedKernel.ValueObjects.Volunteer;

public record SocialMediaDetails
{
    public SocialMediaDetails()
    {
        
    }
    public IReadOnlyList<SocialMedia> SocialMedias { get; }
    public SocialMediaDetails(List<SocialMedia> socialMedias) => SocialMedias = socialMedias;
}