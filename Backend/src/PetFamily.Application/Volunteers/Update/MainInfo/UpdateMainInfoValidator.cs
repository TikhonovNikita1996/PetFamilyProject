using FluentValidation;
using PetFamily.Application.Dtos;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.MainInfo;

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