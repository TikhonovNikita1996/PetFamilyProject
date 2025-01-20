using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.RejectRequest;

public class RejectRequestValidator : AbstractValidator<RejectRequestCommand>
{
    public RejectRequestValidator()
    {
        RuleFor(c => c.RequestId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.RejectionComment).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}