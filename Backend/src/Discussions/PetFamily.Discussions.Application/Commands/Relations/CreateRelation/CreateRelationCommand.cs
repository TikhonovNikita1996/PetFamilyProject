using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.Relations.CreateRelation;

public record CreateRelationCommand (Guid RelationEntityId) : ICommand;