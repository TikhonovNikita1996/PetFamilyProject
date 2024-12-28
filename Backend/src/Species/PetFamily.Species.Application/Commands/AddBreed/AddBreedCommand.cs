using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.AddBreed;

public record AddBreedCommand(Guid SpecieId, string Name) : ICommand;