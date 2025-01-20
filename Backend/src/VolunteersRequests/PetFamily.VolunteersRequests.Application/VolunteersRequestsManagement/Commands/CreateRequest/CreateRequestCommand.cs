using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.CreateRequest;

public record CreateRequestCommand(Guid UserId, string VolunteerInfo) : ICommand;