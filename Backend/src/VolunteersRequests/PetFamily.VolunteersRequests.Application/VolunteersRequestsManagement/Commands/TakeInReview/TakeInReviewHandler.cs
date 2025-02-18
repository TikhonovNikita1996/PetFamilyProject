using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteersRequests.Application.Interfaces;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.TakeInReview;

public class TakeInReviewHandler : ICommandHandler<Guid, TakeInReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<TakeInReviewHandler> _logger;
    private readonly IValidator<TakeInReviewCommand> _validator;
    private readonly IDiscussionContracts _discussionContracts;

    public TakeInReviewHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<TakeInReviewHandler> logger,
        IValidator<TakeInReviewCommand> validator,
        IDiscussionContracts discussionContracts )
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
        _discussionContracts = discussionContracts;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        TakeInReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _requestRepository.GetById(command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound("request").ToErrorList();
        
        var newDiscussionId = await _discussionContracts.CreateDiscussion(command.AdminId,
            existedRequest.Value.UserId, cancellationToken);
            
        existedRequest.Value.TakeInReview(command.AdminId, newDiscussionId.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was taken in review.", command.RequestId);

        return existedRequest.Value.RequestId.Value;
    }
}