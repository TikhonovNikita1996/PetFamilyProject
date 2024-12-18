using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateDonationInfoRequest(Guid VolonteerId,
    UpdateDonationInfoDto UpdateDonationInfoDto);
