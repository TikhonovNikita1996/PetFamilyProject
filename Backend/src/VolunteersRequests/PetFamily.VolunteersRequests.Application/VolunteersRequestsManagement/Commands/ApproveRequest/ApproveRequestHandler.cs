using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ApproveRequest;

public class ApproveRequestHandler : ICommandHandler<Guid, ApproveRequestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<ApproveRequestHandler> _logger;
    private readonly IValidator<ApproveRequestCommand> _validator;
    private readonly IPublisher _publisher;

    public ApproveRequestHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<ApproveRequestHandler> logger,
        IValidator<ApproveRequestCommand> validator,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
        _publisher = publisher;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        ApproveRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _requestRepository.GetById(command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound("request").ToErrorList();
        
        existedRequest.Value.SetApprovedStatus();
        
        await _publisher.PublishDomainEvents(existedRequest.Value, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was approved.", command.RequestId);

        return existedRequest.Value.Id.Value;
    }
}