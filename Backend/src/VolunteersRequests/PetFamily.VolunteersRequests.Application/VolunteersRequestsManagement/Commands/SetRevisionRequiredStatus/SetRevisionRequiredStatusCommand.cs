using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.SetRevisionRequiredStatus;

public record SetRevisionRequiredStatusCommand(Guid RequestId, string RejectionComment) : ICommand;