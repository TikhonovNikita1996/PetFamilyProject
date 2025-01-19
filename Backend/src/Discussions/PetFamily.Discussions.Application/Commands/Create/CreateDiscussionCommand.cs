using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.Create;

public record CreateDiscussionCommand (Guid ReviewingUsersId, Guid ApplicantUserId) : ICommand;