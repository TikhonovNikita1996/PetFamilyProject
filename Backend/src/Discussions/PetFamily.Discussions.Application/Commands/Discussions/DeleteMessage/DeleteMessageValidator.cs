using FluentValidation;

namespace PetFamily.Discussions.Application.Commands.Discussions.DeleteMessage;

public class DeleteMessageValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageValidator()
    {
        RuleFor(c => c.DiscussionId).NotEmpty().NotNull();
        RuleFor(c => c.SenderId).NotEmpty().NotNull();
        RuleFor(c => c.MessageId).NotEmpty().NotNull();
    }
}