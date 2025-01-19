using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Create;

public class CreateDiscussionHandler : ICommandHandler<Discussion, CreateDiscussionCommand>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscussionHandler(IDiscussionRepository discussionRepository,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionRepository = discussionRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Discussion, CustomErrorsList>> Handle(CreateDiscussionCommand request,
        CancellationToken cancellationToken)
    {
        var isDiscussionExists = await _discussionRepository.GetDiscussionById(request.DiscussionId, cancellationToken);
        if (isDiscussionExists.IsSuccess)
            return Errors.General.AlreadyExists("discussion").ToErrorList();
        
        var discussionUsers = DiscussionUsers.Create(request.FirstUsersId, request.SecondUsersId);
        
        var discussion = Discussion.Create(discussionUsers);

        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        var result = await _discussionRepository.Add(discussion.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        return result;
    }
}