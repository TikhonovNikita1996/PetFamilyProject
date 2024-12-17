using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.PetsSpecies.DeleteSpecie;

public record DeleteSpecieCommand(Guid SpecieId) : ICommand;
