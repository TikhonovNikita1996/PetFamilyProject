using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Pet.ValueObjects;

namespace PetFamily.Application.PetsSpecies.Create;

public record CreateSpecieCommand(string Name, List<CreateBreedDto> Breeds) : ICommand;
