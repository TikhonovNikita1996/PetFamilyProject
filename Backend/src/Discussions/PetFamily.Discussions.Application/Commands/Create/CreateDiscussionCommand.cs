using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.Create;

public record CreateDiscussionCommand (Guid DiscussionId, Guid FirstUsersId, Guid SecondUsersId) : ICommand;