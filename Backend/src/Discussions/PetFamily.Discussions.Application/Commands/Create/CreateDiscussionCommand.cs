using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.Create;

public record CreateDiscussionCommand (Guid RelationId, Guid FirstUsersId, Guid SecondUsersId) : ICommand;