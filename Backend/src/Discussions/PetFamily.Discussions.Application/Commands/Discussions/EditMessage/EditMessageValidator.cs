using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Discussions.Application.Commands.Discussions.EditMessage;

public class WriteMessageValidator : AbstractValidator<EditMessageCommand>
{
    public WriteMessageValidator()
    {
        RuleFor(r => r.SenderId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.DiscussionId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.MessageId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.MessageText).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}