using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.WriteMessage;

public class WriteMessageValidator : AbstractValidator<WriteMessageCommand>
{
    public WriteMessageValidator()
    {
        RuleFor(r => r.DiscussionId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.SenderId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.MessageText).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}