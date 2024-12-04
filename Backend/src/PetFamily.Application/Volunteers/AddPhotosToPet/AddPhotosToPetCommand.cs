using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.AddPhotosToPet;

public record AddPhotosToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreateFileDto> Files);