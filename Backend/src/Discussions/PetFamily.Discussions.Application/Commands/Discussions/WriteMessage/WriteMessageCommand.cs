using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.WriteMessage;

public record WriteMessageCommand (Guid DiscussionId, Guid SenderId, string MessageText) : ICommand;