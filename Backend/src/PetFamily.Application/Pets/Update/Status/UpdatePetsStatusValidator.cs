using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.Update.Status;

public class PetSoftDeleteValidator : AbstractValidator<UpdatePetsStatusCommand>
{
    public PetSoftDeleteValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.NewStatus).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}