using FluentValidation;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.MainInfo;

public class UpdateMainInfoValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoValidator()
    {
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateMainInfoDtoValidator : AbstractValidator<UpdateVolunteerMainInfoDto>
{
    public UpdateMainInfoDtoValidator()
    {
        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(x => FullName.Create(x.LastName, x.FirstName, x.MiddleName));
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(c => new { c.WorkingExperience })
            .MustBeValueObject(x => WorkingExperience.Create(x.WorkingExperience));
        
    }
}