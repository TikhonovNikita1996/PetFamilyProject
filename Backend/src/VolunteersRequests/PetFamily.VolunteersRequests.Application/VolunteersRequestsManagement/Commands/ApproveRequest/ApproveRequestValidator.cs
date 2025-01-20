using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.SetRevisionRequiredStatus;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ApproveRequest;

public class SetRejectionStatusValidator : AbstractValidator<ApproveRequestCommand>
{
    public SetRejectionStatusValidator()
    {
        RuleFor(c => c.RequestId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}