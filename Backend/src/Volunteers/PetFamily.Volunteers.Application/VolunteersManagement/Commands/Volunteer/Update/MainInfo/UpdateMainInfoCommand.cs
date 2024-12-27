using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.MainInfo;
public record UpdateMainInfoCommand (Guid VolunteerId, 
    UpdateVolunteerMainInfoDto Dto ) : ICommand;