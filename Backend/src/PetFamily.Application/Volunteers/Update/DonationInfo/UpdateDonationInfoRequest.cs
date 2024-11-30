using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Update.DonationInfo;

public record UpdateDonationInfoRequest(Guid VolonteerId,
    UpdateDonationInfoDto UpdateDonationInfoDto);

public record UpdateDonationInfoDto (
    List<DonationInfoDto> DonationInfos);