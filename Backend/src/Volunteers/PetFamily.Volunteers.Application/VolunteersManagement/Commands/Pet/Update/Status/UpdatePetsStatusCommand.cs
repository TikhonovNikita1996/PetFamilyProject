using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.Status;

public record UpdatePetsStatusCommand (Guid VolunteerId, Guid PetId
    ,string NewStatus) : ICommand;