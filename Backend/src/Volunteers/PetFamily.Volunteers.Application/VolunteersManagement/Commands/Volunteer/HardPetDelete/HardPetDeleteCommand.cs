using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.HardPetDelete;

public record HardPetDeleteCommand(Guid VolunteerId, Guid PetId) : ICommand;