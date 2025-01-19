using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ReopenRequest;

public class RefreshRequestValidator : AbstractValidator<ReopenRequestCommand>
{
    public RefreshRequestValidator()
    {
        RuleFor(c => c.RequestId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}