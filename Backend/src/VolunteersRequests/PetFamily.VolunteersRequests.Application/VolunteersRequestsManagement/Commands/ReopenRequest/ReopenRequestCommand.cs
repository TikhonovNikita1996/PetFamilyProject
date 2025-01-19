using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ReopenRequest;

public record ReopenRequestCommand(Guid RequestId) : ICommand;