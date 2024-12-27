using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.Status;

public class PetSoftDeleteValidator : AbstractValidator<UpdatePetsStatusCommand>
{
    public PetSoftDeleteValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.NewStatus).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}