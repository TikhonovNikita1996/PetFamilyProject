using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Pets.SoftDelete;

public record PetSoftDeleteCommand (Guid VolunteerId, Guid PetId) : ICommand;