using PetFamily.Application.Dtos;

namespace PetFamily.Application.PetsSpecies.AddBreed;

public record AddBreedCommand(Guid SpecieId, BreedDto dto);