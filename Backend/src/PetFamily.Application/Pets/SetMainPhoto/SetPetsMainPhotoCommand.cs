using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.SetMainPhoto;

public record SetPetsMainPhotoCommand (Guid VolunteerId, Guid PetId, FilePath FilePath) : ICommand;