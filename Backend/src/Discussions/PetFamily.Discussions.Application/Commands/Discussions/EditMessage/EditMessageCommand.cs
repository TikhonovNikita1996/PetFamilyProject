using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.EditMessage;

public record EditMessageCommand (Guid DiscussionId, Guid MessageId,
    Guid SenderId, string MessageText) : ICommand;