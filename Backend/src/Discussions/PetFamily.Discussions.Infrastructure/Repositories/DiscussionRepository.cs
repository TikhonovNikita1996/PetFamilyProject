using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Application;
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

    public Task<Discussion?> GetDiscussionById(Guid discussionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Discussion>> GetDiscussionByRelationId(Guid relationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Discussion>> GetDiscussionByStatus(DiscussionStatus status, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}