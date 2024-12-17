using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.PetsSpecies.AddBreed;

public record AddBreedCommand(Guid SpecieId, string Name) : ICommand;