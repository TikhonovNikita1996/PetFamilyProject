using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetsSpecies.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;
