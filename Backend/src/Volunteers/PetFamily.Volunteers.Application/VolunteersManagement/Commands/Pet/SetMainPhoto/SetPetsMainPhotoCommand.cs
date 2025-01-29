using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.SetMainPhoto;

public record SetPetsMainPhotoCommand (Guid VolunteerId, Guid PetId, Guid FileId) : ICommand;