using PetFamily.Core.Abstractions;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Application.Commands.CreateRequest;

public record CreateRequestCommand(Guid UserId, string VolunteerInfo) : ICommand;