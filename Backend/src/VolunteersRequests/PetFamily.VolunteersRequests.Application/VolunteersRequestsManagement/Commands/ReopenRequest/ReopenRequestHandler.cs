using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteersRequests.Application.Interfaces;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ReopenRequest;

public class ReopenRequestHandler : ICommandHandler<Guid, ReopenRequestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRequestRepository _requestRepository;
    private readonly ILogger<ReopenRequestHandler> _logger;
    private readonly IValidator<ReopenRequestCommand> _validator;

    public ReopenRequestHandler(
        [FromKeyedServices(ProjectConstants.Context.VolunteersRequest)] IUnitOfWork unitOfWork,
        IVolunteersRequestRepository requestRepository,
        ILogger<ReopenRequestHandler> logger,
        IValidator<ReopenRequestCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = requestRepository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        ReopenRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var existedRequest = await _requestRepository.GetById(command.RequestId, cancellationToken);
        if (existedRequest.IsFailure)
            return Errors.General.NotFound("request").ToErrorList();
            
        existedRequest.Value.Refresh();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer request with id {requestId} was reopened with submitted status."
            , command.RequestId);

        return existedRequest.Value.RequestId;
    }
}