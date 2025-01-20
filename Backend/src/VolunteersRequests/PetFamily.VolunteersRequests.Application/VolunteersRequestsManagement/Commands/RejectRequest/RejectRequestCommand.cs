using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.RejectRequest;

public record RejectRequestCommand(Guid RequestId, string RejectionComment) : ICommand;