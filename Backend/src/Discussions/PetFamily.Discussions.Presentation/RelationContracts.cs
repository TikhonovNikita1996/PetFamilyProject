using PetFamily.Discussions.Contracts;

namespace PetFamily.Discussions.Presentation;

public class RelationContracts : IRelationContracts
{
    public Task<bool> IsRelationExist(Guid RelationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}