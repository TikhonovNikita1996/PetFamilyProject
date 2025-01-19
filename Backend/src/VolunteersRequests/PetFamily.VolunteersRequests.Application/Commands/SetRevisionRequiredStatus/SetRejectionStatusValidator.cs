using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.VolunteersRequests.Application.Commands.SetRevisionRequiredStatus;

public class SetRejectionStatusValidator : AbstractValidator<SetRejectionStatusCommand>
{
    public SetRejectionStatusValidator()
    {
        RuleFor(c => c.RejectionComment).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.RequestId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}