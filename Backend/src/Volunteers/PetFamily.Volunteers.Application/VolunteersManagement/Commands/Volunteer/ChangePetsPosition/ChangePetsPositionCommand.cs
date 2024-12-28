using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.ChangePetsPosition;

public record ChangePetsPositionCommand(Guid VolunteerId, Guid PetId, int NewPosition) : ICommand;