using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid UserId,
    List<SocialMediaDetailsDto> SocialMediaDetailsDtos) : ICommand;
