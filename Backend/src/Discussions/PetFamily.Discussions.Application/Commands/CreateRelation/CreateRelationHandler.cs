using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.CreateRelation;

public class CreateRelationHandler : ICommandHandler<Discussion, CreateRelationCommand>
{
    private readonly IDiscussionRepository _discussionRepository;

    public CreateRelationHandler(IDiscussionRepository discussionRepository)
    {
        _discussionRepository = discussionRepository;
    }
    
    public async Task<Result<Discussion, CustomErrorsList>> Handle(CreateRelationCommand request,
        CancellationToken cancellationToken)
    {
        var isDiscussionExists = await _discussionsRepository.GetByRelationId(request.RelationId, cancellationToken);
        if (isDiscussionExists.IsSuccess)
            return Errors.General.AlreadyExist("Discussion").ToErrorList();
        
        var discussion = Discussion.Create(request.RelationId, members);

        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        var result = await _discussionsRepository.Add(discussion.Value, cancellationToken);
        return result;
    }
}