using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;
using PetFamily.Discussions.Infrastructure.DataContexts;

namespace PetFamily.Discussions.Infrastructure.Repositories;

public class RelationRepository : IRelationRepository
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public RelationRepository(DiscussionsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Relation> Add(Relation relation,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Relations.Add(relation);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return relation;
    }

    public async Task Remove(Relation relation,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Relations.Remove(relation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Relation?> GetRelationById(Guid relationId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Relations
            .Include(r => r.Discussions)
            .SingleOrDefaultAsync(r => r.RelationId == relationId, cancellationToken);
    }
}