using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.CreateRelation;

public record CreateRelationCommand (Guid PetId) : ICommand;