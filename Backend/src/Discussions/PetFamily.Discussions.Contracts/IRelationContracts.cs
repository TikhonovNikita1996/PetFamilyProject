namespace PetFamily.Discussions.Contracts;

public interface IRelationContracts
{
    Task<bool> IsRelationExist(Guid RelationId,
        CancellationToken cancellationToken = default);
}