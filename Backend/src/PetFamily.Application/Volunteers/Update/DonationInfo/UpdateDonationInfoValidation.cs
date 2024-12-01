using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;

namespace PetFamily.Application.Volunteers.Update.DonationInfo;

public class UpdateDonationInfoValidation : AbstractValidator<UpdateDonationInfoRequest>
{
    public UpdateDonationInfoValidation()
    {
        RuleFor(r => r.VolonteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(r => r.UpdateDonationInfoDto.DonationInfos)
            .MustBeValueObject(vo => Domain.Entities.Volunteer.ValueObjects.DonationInfo.Create(vo.Name, vo.Description));
    }
}