using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.CreateDiscussion;

public class CreateDiscussionHandler : ICommandHandler<Discussion, CreateDiscussionCommand>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IDiscussionsReadDbContext _discussionsReadDbContext;
    private readonly IValidator<CreateDiscussionCommand> _validator;
    private readonly ILogger<CreateDiscussionHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscussionHandler(IDiscussionRepository discussionRepository,
        IDiscussionsReadDbContext discussionsReadDbContext,
        IValidator<CreateDiscussionCommand> validator,
        ILogger<CreateDiscussionHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionRepository = discussionRepository;
        _discussionsReadDbContext = discussionsReadDbContext;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Discussion, CustomErrorsList>> Handle(CreateDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var discussionUsers = DiscussionUsers.Create(command.ReviewingUsersId, command.ApplicantUserId);
        
        var discussion = Discussion.Create(discussionUsers);

        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        var result = await _discussionRepository.Add(discussion.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Discussion added with id {discussionId}.", discussion.Value.DiscussionId);
        
        return result;
    }
}