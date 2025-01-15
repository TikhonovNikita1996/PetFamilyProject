using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Accounts.Presentation.Requests;

public record UpdateSocialNetworksRequest(List<SocialMediaDetailsDto> SocialMediaDetails);