using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.DonationInfo;

public record UpdateDonationInfoCommand(Guid VolonteerId,
    UpdateDonationInfoDto UpdateDonationInfoDto) : ICommand;