using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Specie;

namespace PetFamily.Species.Application.Commands.Create;

public record CreateSpecieCommand(string Name, List<CreateBreedDto> Breeds) : ICommand;
