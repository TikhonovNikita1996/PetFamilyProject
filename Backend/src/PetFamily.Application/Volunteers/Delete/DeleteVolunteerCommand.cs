using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;