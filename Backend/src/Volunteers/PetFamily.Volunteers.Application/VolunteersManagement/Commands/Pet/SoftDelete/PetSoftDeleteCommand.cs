using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.SoftDelete;

public record PetSoftDeleteCommand (Guid VolunteerId, Guid PetId) : ICommand;