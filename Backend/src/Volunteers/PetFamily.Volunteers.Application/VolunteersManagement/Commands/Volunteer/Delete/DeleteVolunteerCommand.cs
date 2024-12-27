using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;