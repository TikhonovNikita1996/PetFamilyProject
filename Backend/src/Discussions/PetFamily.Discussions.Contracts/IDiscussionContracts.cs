using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Application.Commands.Create;

namespace PetFamily.Discussions.Contracts;

public interface IDiscussionContracts
{
    public Task<Result<Guid, CustomErrorsList>> CreateDiscussion(
        CreateDiscussionCommand command,
        CancellationToken cancellationToken = default);
}