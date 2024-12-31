using FluentValidation;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Create;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.PhoneNumber).MustBeValueObject(pn => PhoneNumber.Create(pn.Value));
        RuleFor(c => c.Description).MustBeValueObject(d => Description.Create(d.Value));
    }
}