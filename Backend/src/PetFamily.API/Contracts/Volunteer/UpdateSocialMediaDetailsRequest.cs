using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateSocialMediaDetailsRequest(Guid VolonteerId,
    UpdateSocialNetworksDto UpdateSocialNetworksDto);
