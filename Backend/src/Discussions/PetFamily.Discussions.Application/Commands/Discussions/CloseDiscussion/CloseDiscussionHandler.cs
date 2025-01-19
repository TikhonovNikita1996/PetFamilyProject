using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.CloseDiscussion;

public class CloseDiscussionHandler : ICommandHandler<Guid, CloseDiscussionCommand>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IValidator<CloseDiscussionCommand> _validator;
    private readonly ILogger<CloseDiscussionHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CloseDiscussionHandler(IDiscussionRepository discussionRepository,
        IDiscussionsReadDbContext discussionsReadDbContext,
        IValidator<CloseDiscussionCommand> validator,
        ILogger<CloseDiscussionHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionRepository = discussionRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(CloseDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        var existedDiscussion = await _discussionRepository.GetDiscussionById(command.DiscussionId, cancellationToken);
        
        if (existedDiscussion.IsFailure)
            return Errors.General.NotFound("discussion").ToErrorList();

        existedDiscussion.Value.Close();

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Discussion closed with id {discussionId}.", existedDiscussion.Value.DiscussionId);
        
        return existedDiscussion.Value.DiscussionId;
    }
}