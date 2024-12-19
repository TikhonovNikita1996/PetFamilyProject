using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Pets.Update.MainInfo;

public record UpdatePetsMainInfoCommand (Guid VolunteerId, Guid PetId, 
    UpdatePetsMainInfoDto Dto ) : ICommand;