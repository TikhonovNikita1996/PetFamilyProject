using PetFamily.Core.Dtos.Specie;

namespace PetFamily.Species.Presentation.Specie;

public record CreateSpecieRequest(string Name, List<CreateBreedDto> Breeds);
