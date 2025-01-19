using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Discussions.Application.Commands.Discussions.CloseDiscussion;

public class CloseDiscussionValidator : AbstractValidator<CloseDiscussionCommand>
{
    public CloseDiscussionValidator()
    {
        RuleFor(r => r.DiscussionId).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}