using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Discussions.Application.Commands.Discussions.DeleteMessage;

public class DeleteMessageValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageValidator()
    {
        RuleFor(c => c.DiscussionId).NotEmpty().NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.SenderId).NotEmpty().NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.MessageId).NotEmpty().NotNull().WithError(Errors.General.ValueIsRequired());
    }
}