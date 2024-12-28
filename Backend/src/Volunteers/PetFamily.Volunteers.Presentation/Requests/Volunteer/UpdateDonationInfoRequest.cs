using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateDonationInfoRequest(Guid VolonteerId,
    UpdateDonationInfoDto UpdateDonationInfoDto);
