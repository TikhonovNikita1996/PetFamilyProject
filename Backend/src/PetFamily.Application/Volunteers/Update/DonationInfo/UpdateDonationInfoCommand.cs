using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Update.DonationInfo;

public record UpdateDonationInfoCommand(Guid VolonteerId,
    UpdateDonationInfoDto UpdateDonationInfoDto) : ICommand;