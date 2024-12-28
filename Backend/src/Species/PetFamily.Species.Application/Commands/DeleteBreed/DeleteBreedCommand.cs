using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;
