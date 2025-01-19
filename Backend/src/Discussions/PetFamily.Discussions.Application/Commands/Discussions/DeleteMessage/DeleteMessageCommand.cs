using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.DeleteMessage;

public record DeleteMessageCommand (Guid DiscussionId, Guid MessageId , Guid SenderId) : ICommand;