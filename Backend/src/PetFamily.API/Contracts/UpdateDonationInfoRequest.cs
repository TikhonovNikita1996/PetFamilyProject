using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts;

public record UpdateDonationInfoRequest(Guid VolonteerId,
    UpdateDonationInfoDto UpdateDonationInfoDto);
