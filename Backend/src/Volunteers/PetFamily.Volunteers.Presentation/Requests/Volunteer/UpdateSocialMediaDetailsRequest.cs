using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateSocialMediaDetailsRequest(Guid VolonteerId,
    UpdateSocialNetworksDto UpdateSocialNetworksDto);
