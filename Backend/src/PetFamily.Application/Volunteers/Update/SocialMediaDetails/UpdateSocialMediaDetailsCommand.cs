using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Update.SocialMediaDetails;

public record UpdateSocialMediaDetailsCommand(Guid VolonteerId,
    UpdateSocialNetworksDto UpdateSocialNetworksDto);