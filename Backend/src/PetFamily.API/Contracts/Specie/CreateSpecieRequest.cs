using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Specie;

public record CreateSpecieRequest(string Name, List<CreateBreedDto> Breeds);
