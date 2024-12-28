using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Create;

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