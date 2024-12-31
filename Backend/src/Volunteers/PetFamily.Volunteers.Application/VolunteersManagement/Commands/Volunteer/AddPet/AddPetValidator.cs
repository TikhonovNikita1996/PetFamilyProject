using FluentValidation;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPet;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(fn => PetsName.Create(fn.Name));
        RuleFor(c => c.Gender).NotNull().Must(g => g is "Male" or "Female")
            .WithError(Errors.General.ValueIsInvalid("Gender"));
        RuleFor(c => c.Description).MustBeValueObject(d => PetsDescription.Create(d.Value));
        RuleFor(c => c.HealthInformation).MustBeValueObject(d => HealthInformation.Create(d.Value));
        RuleFor(c => c.OwnersPhoneNumber).MustBeValueObject(d => OwnersPhoneNumber.Create(d.Value));
        RuleFor(c => c.Color).NotNull().NotEmpty();
        RuleFor(c => c.Weight).NotNull().Must(weight => weight > 0).WithError(Errors.General.ValueIsInvalid("Weight"));
        RuleFor(c => c.Height).NotNull().Must(height => height > 0).WithError(Errors.General.ValueIsInvalid("Height"));
    }
}