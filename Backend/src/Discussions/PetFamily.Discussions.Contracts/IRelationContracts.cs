using System.Globalization;

namespace PetFamily.Discussions.Contracts;

public interface IRelationContracts
{
    Task<bool> IsRelationExist(Guid RelationId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateRelation(Guid id,
        CancellationToken cancellationToken = default);
}