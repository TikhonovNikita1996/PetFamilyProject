using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Update.SocialMediaDetails;

public record UpdateSocialMediaDetailsRequest(Guid VolonteerId,
    UpdateSocialNetworksDto UpdateSocialNetworksDto);
    
public record UpdateSocialNetworksDto(
    List<SocialMediaDetailsDto> SocialNetworks);