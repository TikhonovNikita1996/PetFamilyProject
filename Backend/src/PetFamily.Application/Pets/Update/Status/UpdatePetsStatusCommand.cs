using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Pets.Update.Status;

public record UpdatePetsStatusCommand (Guid VolunteerId, Guid PetId
    ,string NewStatus) : ICommand;