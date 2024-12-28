namespace PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

public record SocialMediaDetails
{
    public SocialMediaDetails()
    {
        
    }
    public IReadOnlyList<SocialMedia> SocialMedias { get; }
    public SocialMediaDetails(List<SocialMedia> socialMedias) => SocialMedias = socialMedias;
}