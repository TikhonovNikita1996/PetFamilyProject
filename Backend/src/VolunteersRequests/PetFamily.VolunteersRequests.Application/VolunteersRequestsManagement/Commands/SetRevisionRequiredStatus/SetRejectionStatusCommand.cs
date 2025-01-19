using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.SetRevisionRequiredStatus;

public record SetRejectionStatusCommand(Guid RequestId, string RejectionComment) : ICommand;