using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Discussions.Application.Commands.Discussions.CreateDiscussion;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Contracts;
using PetFamily.Framework;

namespace PetFamily.Discussions.Presentation;

public class DiscussionContracts : IDiscussionContracts
{
    private readonly CreateDiscussionHandler _createDiscussionHandler;

    public DiscussionContracts(CreateDiscussionHandler createDiscussionHandler)
    {
        _createDiscussionHandler = createDiscussionHandler;
    }
    public async Task<Result<Guid, CustomErrorsList>> CreateDiscussion(Guid reviewingUserId, Guid applicantUserId,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateDiscussionCommand(reviewingUserId, applicantUserId);
        var discussion = await _createDiscussionHandler.Handle(createCommand, cancellationToken);
        if (discussion.IsFailure)
            return discussion.Error;
        
        return discussion.Value.DiscussionId;
    }
    
}