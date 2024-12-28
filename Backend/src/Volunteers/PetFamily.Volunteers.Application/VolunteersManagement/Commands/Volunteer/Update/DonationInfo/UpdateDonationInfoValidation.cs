using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.DonationInfo;

public class UpdateDonationInfoValidation : AbstractValidator<UpdateDonationInfoCommand>
{
    public UpdateDonationInfoValidation()
    {
        RuleFor(r => r.VolonteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(r => r.UpdateDonationInfoDto.DonationInfos)
            .MustBeValueObject(vo => Domain.Volunteer.ValueObjects.DonationInfo.Create(vo.Name, vo.Description));
    }
}