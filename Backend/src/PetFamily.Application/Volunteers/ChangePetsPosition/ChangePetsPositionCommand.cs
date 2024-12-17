using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.ChangePetsPosition;

public record ChangePetsPositionCommand(Guid VolunteerId, Guid PetId, int NewPosition) : ICommand;