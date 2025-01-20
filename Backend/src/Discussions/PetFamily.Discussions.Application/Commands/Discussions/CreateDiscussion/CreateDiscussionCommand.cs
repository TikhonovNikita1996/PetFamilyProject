using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.Discussions.CreateDiscussion;

public record CreateDiscussionCommand (Guid ReviewingUsersId, Guid ApplicantUserId) : ICommand;