using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.SocialMediaDetails;

public class UpdateSocialMediaDetailsValidator : AbstractValidator<UpdateSocialMediaDetailsRequest>
{
    public UpdateSocialMediaDetailsValidator()
    {
        RuleFor(r => r.VolonteerId).NotNull().WithError(Errors.General.ValueIsRequired());

        RuleForEach(r => r.UpdateSocialNetworksDto.SocialNetworks)
            .MustBeValueObject(vo => SocialMedia.Create(vo.Name, vo.Url));
    }

}