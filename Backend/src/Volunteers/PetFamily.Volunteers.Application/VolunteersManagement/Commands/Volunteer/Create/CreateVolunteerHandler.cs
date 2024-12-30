using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Domain.Ids;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid,CreateVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.VolunteerManagement)]IUnitOfWork unitOfWork, 
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<Result<Guid, CustomErrorsList>> Handle(CreateVolunteerCommand createVolunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(createVolunteerCommand);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunterId = VolunteerId.NewId();
        
        var description = Description.Create(createVolunteerCommand.Description.Value).Value;
        
        var phoneNumber = PhoneNumber.Create(createVolunteerCommand.PhoneNumber.Value).Value;
        
        var volunteer = Domain.Volunteer.Volunteer.Create(volunterId, description, phoneNumber);
        
        if((volunteer.IsFailure))
            return volunteer.Error.ToErrorList();
        
        await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created volunteer with ID: {volunteer.Value.Id}", volunteer.Value.Id);
        
        return volunteer.Value.Id.Value;
    }
}