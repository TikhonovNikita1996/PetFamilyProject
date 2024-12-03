using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Update.SocialMediaDetails;

namespace PetFamily.API.Contracts;

public record UpdateSocialMediaDetailsRequest(Guid VolonteerId,
    UpdateSocialNetworksDto UpdateSocialNetworksDto);
