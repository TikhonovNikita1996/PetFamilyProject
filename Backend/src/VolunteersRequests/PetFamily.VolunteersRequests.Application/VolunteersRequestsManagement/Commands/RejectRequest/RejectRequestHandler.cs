using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.RejectRequest;

public class RejectRequestHandler : ICommandHandler<Guid, RejectRequestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<RejectRequestHandler> _logger;
    private readonly IValidator<RejectRequestCommand> _validator;
    private readonly IAccountContracts _accountContracts;

    public RejectRequestHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<RejectRequestHandler> logger,
        IValidator<RejectRequestCommand> validator,
        IAccountContracts accountContracts)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
        _accountContracts = accountContracts;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        RejectRequestCommand command,
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
        existedRequest.Value.SetRejectStatus(rejectionComment);

        await _accountContracts.BanUser(existedRequest.Value.UserId, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was rejected."
            , command.RequestId);

        return existedRequest.Value.RequestId;
    }
}