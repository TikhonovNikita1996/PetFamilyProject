using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Discussions.Contracts;

public interface IDiscussionContracts
{
    public Task<Result<Guid, CustomErrorsList>> CreateDiscussion(Guid reviewingUserId,
        Guid applicantUserId,
        CancellationToken cancellationToken = default);
}