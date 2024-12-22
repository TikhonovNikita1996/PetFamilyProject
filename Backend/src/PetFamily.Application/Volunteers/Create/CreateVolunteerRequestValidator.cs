using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.FullName).MustBeValueObject(fn => FullName.Create(fn.LastName, fn.Name, fn.MiddleName));
        RuleFor(c => c.Gender).NotNull().Must(g => g is "Male" or "Female")
            .WithError(Errors.General.ValueIsInvalid("Gender"));
        RuleFor(c => c.Email).MustBeValueObject(m => Email.Create(m.Value));
        RuleFor(c => c.WorkingExperience).MustBeValueObject(we => WorkingExperience.Create(we.Value));
        RuleFor(c => c.PhoneNumber).MustBeValueObject(pn => PhoneNumber.Create(pn.Value));
        RuleFor(c => c.Description).MustBeValueObject(d => Description.Create(d.Value));
    }
}