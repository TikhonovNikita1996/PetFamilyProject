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

namespace PetFamily.Discussions.Application.Commands.Discussions.WriteMessage;

public class WriteMessageHandler : ICommandHandler<Guid, WriteMessageCommand>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IDiscussionsReadDbContext _discussionsReadDbContext;
    private readonly IValidator<WriteMessageCommand> _validator;
    private readonly ILogger<WriteMessageHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public WriteMessageHandler(IDiscussionRepository discussionRepository,
        IDiscussionsReadDbContext discussionsReadDbContext,
        IValidator<WriteMessageCommand> validator,
        ILogger<WriteMessageHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionRepository = discussionRepository;
        _discussionsReadDbContext = discussionsReadDbContext;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(WriteMessageCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        var discussion = await _discussionRepository.GetDiscussionById(command.DiscussionId, cancellationToken);
        
        if (discussion.IsFailure)
            return Errors.General.NotFound("discussion").ToErrorList();
        
        var messageText = MessageText.Create(command.MessageText).Value;
        
        var message = Message.Create(command.DiscussionId, command.SenderId, messageText);
        
        discussion.Value.AddMessage(message);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Message was added in discussion with id {discussionId}.",
            discussion.Value.DiscussionId);
        
        return message.MessageId;
    }
}