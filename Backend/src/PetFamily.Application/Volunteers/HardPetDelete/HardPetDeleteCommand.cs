using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.HardPetDelete;

public record HardPetDeleteCommand(Guid VolunteerId, Guid PetId) : ICommand;