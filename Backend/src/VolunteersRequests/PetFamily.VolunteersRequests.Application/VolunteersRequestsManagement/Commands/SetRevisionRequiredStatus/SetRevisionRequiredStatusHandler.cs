using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.SetRevisionRequiredStatus;

public class SetRevisionRequiredStatusHandler : ICommandHandler<Guid, SetRevisionRequiredStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<SetRevisionRequiredStatusHandler> _logger;
    private readonly IValidator<SetRevisionRequiredStatusCommand> _validator;
    private readonly IDiscussionContracts _discussionContracts;

    public SetRevisionRequiredStatusHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<SetRevisionRequiredStatusHandler> logger,
        IValidator<SetRevisionRequiredStatusCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        SetRevisionRequiredStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _requestRepository.GetById(command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound("request").ToErrorList();
            
        var rejectionComment = RejectionComment.Create(command.RejectionComment).Value;
        existedRequest.Value.SetRevisionRequiredStatus(rejectionComment);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was rejected.", command.RequestId);

        return existedRequest.Value.RequestId.Value;
    }
}