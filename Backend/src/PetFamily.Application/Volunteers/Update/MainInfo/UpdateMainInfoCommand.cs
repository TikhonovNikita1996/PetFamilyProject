using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Ids;

namespace PetFamily.Application.Volunteers.Update.MainInfo;
public record UpdateMainInfoCommand (Guid VolunteerId, 
    UpdateMainInfoDto Dto ) : ICommand;