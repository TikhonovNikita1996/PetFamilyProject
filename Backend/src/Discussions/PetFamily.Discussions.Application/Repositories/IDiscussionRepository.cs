using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Repositories;

public interface IDiscussionRepository
{
    public Task<Discussion> Add(Discussion discussion, CancellationToken cancellationToken);
    public Task<Result<Guid, CustomError>> Remove(Discussion discussion, CancellationToken cancellationToken);
    public Task<Result<Discussion, CustomError>> GetDiscussionById(Guid discussionId,
        CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<Discussion>> GetDiscussionsByStatus(DiscussionStatus status,
        CancellationToken cancellationToken = default);
}