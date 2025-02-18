using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.CreateRequest;

public class CreateRequestHandler : ICommandHandler<Guid, CreateRequestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<CreateRequestHandler> _logger;
    private readonly IValidator<CreateRequestCommand> _validator;
    private readonly IAccountContracts _accountContracts;

    public CreateRequestHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<CreateRequestHandler> logger,
        IValidator<CreateRequestCommand> validator,
        IAccountContracts accountContracts)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
        _accountContracts = accountContracts;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        CreateRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        bool isUserBanned = await _accountContracts.IsUserBannedForVolunteerRequests(command.UserId, cancellationToken);
        if (isUserBanned)
            return Errors.General.Failure("user is banned").ToErrorList();
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var userId = command.UserId;
        var volunteerInfo = VolunteerInfo.Create(command.VolunteerInfo).Value;
        var requestId = VolunteerRequestId.NewId();
        var newRequest = VolunteerRequest.Create(requestId, userId, volunteerInfo).Value;

        await _requestRepository.Add(newRequest, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request was added with id {requestId}.", requestId);

        return newRequest.RequestId.Value;
    }
}