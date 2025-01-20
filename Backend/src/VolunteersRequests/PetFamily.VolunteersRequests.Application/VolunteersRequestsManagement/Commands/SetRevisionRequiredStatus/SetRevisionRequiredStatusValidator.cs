using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.SetRevisionRequiredStatus;

public class SetRevisionRequiredStatusValidator : AbstractValidator<SetRevisionRequiredStatusCommand>
{
    public SetRevisionRequiredStatusValidator()
    {
        RuleFor(c => c.RejectionComment).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.RequestId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}