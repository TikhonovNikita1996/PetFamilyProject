using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Entities.Others;

public record SocialMediaDetails
{
    public SocialMediaDetails()
    {
        
    }
    public IReadOnlyList<SocialMedia> SocialMedias { get; }
    public SocialMediaDetails(List<SocialMedia> socialMedias) => SocialMedias = socialMedias;
}