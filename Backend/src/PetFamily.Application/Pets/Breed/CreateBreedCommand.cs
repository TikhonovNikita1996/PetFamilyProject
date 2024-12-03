using PetFamily.Domain.Entities.Ids;

namespace PetFamily.Application.Pets.Breed;

public record CreateBreedCommand(SpecieId SpecieId, string Name);