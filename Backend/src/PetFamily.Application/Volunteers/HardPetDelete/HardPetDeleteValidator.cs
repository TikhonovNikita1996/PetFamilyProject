using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.HardPetDelete;

public class HardPetDeleteValidator : AbstractValidator<HardPetDeleteCommand>
{
    public HardPetDeleteValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}