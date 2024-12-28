using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.SocialMediaDetails;

public class UpdateSocialMediaDetailsValidator : AbstractValidator<UpdateSocialMediaDetailsCommand>
{
    public UpdateSocialMediaDetailsValidator()
    {
        RuleFor(r => r.VolonteerId).NotNull().WithError(Errors.General.ValueIsRequired());

        RuleForEach(r => r.UpdateSocialNetworksDto.SocialNetworks)
            .MustBeValueObject(vo => SocialMedia.Create(vo.Name, vo.Url));
    }

}