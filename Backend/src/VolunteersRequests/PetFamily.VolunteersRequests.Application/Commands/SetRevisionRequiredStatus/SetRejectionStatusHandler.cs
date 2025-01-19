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

namespace PetFamily.VolunteersRequests.Application.Commands.SetRevisionRequiredStatus;

public class SetRejectionStatusHandler : ICommandHandler<Guid, SetRejectionStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<SetRejectionStatusHandler> _logger;
    private readonly IValidator<SetRejectionStatusCommand> _validator;
    private readonly IDiscussionContracts _discussionContracts;

    public SetRejectionStatusHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<SetRejectionStatusHandler> logger,
        IValidator<SetRejectionStatusCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        SetRejectionStatusCommand command,
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

        return existedRequest.Value.RequestId;
    }
}