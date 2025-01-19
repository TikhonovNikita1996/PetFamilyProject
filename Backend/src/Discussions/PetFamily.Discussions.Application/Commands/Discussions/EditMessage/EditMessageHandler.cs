using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.Discussions.EditMessage;

public class EditMessageHandler : ICommandHandler<Guid, EditMessageCommand>
{
    private readonly IDiscussionRepository _discussionRepository;
    private readonly IValidator<EditMessageCommand> _validator;
    private readonly ILogger<EditMessageHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public EditMessageHandler(IDiscussionRepository discussionRepository,
        IDiscussionsReadDbContext discussionsReadDbContext,
        IValidator<EditMessageCommand> validator,
        ILogger<EditMessageHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _discussionRepository = discussionRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(EditMessageCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        var discussion = await _discussionRepository.GetDiscussionById(command.DiscussionId, cancellationToken);
        
        if (discussion.IsFailure)
            return Errors.General.NotFound("discussion").ToErrorList();

        var messageText = MessageText.Create(command.MessageText).Value;
        
        discussion.Value.EditMessage(command.MessageId, command.SenderId, messageText);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Message was edited in discussion with id {discussionId}.",
            discussion.Value.DiscussionId);
        
        return command.MessageId;
    }
}