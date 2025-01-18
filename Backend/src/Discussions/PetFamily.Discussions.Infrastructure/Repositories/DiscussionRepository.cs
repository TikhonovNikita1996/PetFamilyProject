using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;
using PetFamily.Discussions.Infrastructure.DataContexts;

namespace PetFamily.Discussions.Infrastructure.Repositories;

public class DiscussionRepository : IDiscussionRepository
{
    private readonly DiscussionsWriteDbContext _dbContext;

    public DiscussionRepository(DiscussionsWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Discussion> Add(Discussion discussion,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Discussions.Add(discussion);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return discussion;
    }

    public async Task<Result<Guid, CustomError>> Remove(Discussion discussion,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Discussions.Remove(discussion);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return discussion.DiscussionId;
    }

    public async Task<Result<Discussion, CustomError>> GetDiscussionById(Guid discussionId,
        CancellationToken cancellationToken = default)
    {
        var discussion = await _dbContext.Discussions
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.DiscussionId == discussionId, cancellationToken);

        if (discussion == null)
            return Errors.General.NotFound("discussion");
        
        return discussion;
    }

    public async Task<IReadOnlyList<Discussion>> GetDiscussionsByRelationId(Guid relationId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Discussions
            .Include(d => d.Messages)
            .Where(d => d.RelationId == relationId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Discussion>> GetDiscussionsByStatus(DiscussionStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Discussions
            .Include(d => d.Messages)
            .Where(d => d.Status == status).ToListAsync(cancellationToken);
    }
}