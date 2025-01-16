using FluentValidation;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksValidator()
    {
        RuleFor(c => c.UserId).NotNull().NotEmpty();
        RuleFor(c => c.SocialMediaDetailsDtos).NotNull().NotEmpty();
    }
}