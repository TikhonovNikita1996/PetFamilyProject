using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Discussions.Application.Commands.Discussions.CreateDiscussion;

public class CreateDiscussionValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionValidator()
    {
        RuleFor(c => c.ApplicantUserId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.ReviewingUsersId).NotNull().NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}