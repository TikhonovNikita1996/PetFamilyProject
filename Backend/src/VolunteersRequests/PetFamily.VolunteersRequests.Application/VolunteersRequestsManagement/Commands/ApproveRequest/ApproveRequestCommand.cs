using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ApproveRequest;

public record ApproveRequestCommand(Guid RequestId) : ICommand;