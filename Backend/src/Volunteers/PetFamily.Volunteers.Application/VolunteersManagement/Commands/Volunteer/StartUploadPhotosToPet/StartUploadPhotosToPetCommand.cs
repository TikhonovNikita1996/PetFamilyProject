using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.StartUploadPhotosToPet;

public record StartUploadPhotosToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreateFileDto> Files) : ICommand;