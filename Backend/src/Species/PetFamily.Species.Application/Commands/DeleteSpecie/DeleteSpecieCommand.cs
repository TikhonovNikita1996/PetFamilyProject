using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.DeleteSpecie;

public record DeleteSpecieCommand(Guid SpecieId) : ICommand;
