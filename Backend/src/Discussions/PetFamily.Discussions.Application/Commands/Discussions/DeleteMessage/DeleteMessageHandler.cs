using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.DeleteMessage;

public class DeleteMessageHandler : ICommandHandler<Guid, DeleteMessageCommand>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IDiscussionsReadDbContext _discussionsReadDbContext;
    private readonly IValidator<DeleteMessageCommand> _validator;
    private readonly ILogger<DeleteMessageHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMessageHandler(IDiscussionRepository discussionRepository,
        IDiscussionsReadDbContext discussionsReadDbContext,
        IValidator<DeleteMessageCommand> validator,
        ILogger<DeleteMessageHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionRepository = discussionRepository;
        _discussionsReadDbContext = discussionsReadDbContext;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(DeleteMessageCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        var discussion = await _discussionRepository.GetDiscussionById(command.DiscussionId, cancellationToken);
        
        if (discussion.IsFailure)
            return Errors.General.NotFound("discussion").ToErrorList();
        
        discussion.Value.RemoveMessage(command.MessageId, command.SenderId);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Message was deleted from discussion with id {discussionId}.",
            discussion.Value.DiscussionId);
        
        return command.MessageId;
    }
}