using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Pet;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.MainInfo;

public record UpdatePetsMainInfoCommand (Guid VolunteerId, Guid PetId, 
    UpdatePetsMainInfoDto Dto ) : ICommand;