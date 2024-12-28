using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.SocialMediaDetails;

public record UpdateSocialMediaDetailsCommand(Guid VolonteerId,
    UpdateSocialNetworksDto UpdateSocialNetworksDto) : ICommand;