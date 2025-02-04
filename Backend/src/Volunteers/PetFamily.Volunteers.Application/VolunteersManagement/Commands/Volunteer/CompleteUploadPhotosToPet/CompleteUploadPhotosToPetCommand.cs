using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Photos;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.CompleteUploadPhotosToPet;

public record CompleteUploadPhotosToPetCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CompleteFileUploadDto> Dtos) : ICommand;