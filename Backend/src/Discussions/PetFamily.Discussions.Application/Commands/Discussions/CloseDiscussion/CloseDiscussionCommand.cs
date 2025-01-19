using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.Discussions.CloseDiscussion;

public record CloseDiscussionCommand (Guid DiscussionId) : ICommand;