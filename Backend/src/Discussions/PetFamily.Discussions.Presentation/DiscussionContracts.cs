using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Application.Commands.Create;
using PetFamily.Discussions.Contracts;

namespace PetFamily.Discussions.Presentation;

public class DiscussionContracts : IDiscussionContracts
{
    public Task<Result<Guid, CustomErrorsList>> CreateDiscussion(CreateDiscussionCommand command,
        CancellationToken cancellationToken = default)
    {
        
        throw new NotImplementedException();
    }
}