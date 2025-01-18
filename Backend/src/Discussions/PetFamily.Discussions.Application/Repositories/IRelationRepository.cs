using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Repositories;

public interface IRelationRepository
{
    public Task<Relation> Add(Relation relation, CancellationToken cancellationToken);
    public Task Remove(Relation relation, CancellationToken cancellationToken);

    public Task<Relation?> GetRelationById(Guid relationId,
        CancellationToken cancellationToken = default);
    
    public Task<Result<Relation, CustomError>> GetByRelatedEntityId(Guid id,
        CancellationToken cancellationToken = default);
}