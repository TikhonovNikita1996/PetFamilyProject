using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.VolunteersRequests.Application.Interfaces;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.TakeInReview;

public class TakeInReviewHandler : ICommandHandler<Guid, TakeInReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<TakeInReviewHandler> _logger;
    private readonly IValidator<TakeInReviewCommand> _validator;
    private readonly IPublisher _publisher;

    public TakeInReviewHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<TakeInReviewHandler> logger,
        IValidator<TakeInReviewCommand> validator,
        IPublisher publisher )
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
        _publisher = publisher;
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

        existedRequest.Value.TakeInReview(command.AdminId);
        
        await _publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was taken in review.", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}