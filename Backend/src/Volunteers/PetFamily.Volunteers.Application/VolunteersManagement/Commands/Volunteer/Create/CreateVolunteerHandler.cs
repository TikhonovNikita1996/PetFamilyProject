using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Domain.Ids;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

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
        
        var email = Email.Create(createVolunteerCommand.Email.Value).Value;
        
        var volunterId = VolunteerId.NewId();
        var fullName = FullName.Create(createVolunteerCommand.FullName.LastName, 
            createVolunteerCommand.FullName.Name, createVolunteerCommand.FullName.MiddleName).Value;
        
        var workingExperience = WorkingExperience.Create(createVolunteerCommand.WorkingExperience.Value).Value;
        
        var description = Description.Create(createVolunteerCommand.Description.Value).Value;
        
        var phoneNumber = PhoneNumber.Create(createVolunteerCommand.PhoneNumber.Value).Value;
        
        var gender = Enum.Parse<GenderType>(createVolunteerCommand.Gender);
        
        var volunteer = Domain.Volunteer.Volunteer.Create(volunterId, fullName, email, gender,
            workingExperience, description, phoneNumber);
        
        if((volunteer.IsFailure))
            return volunteer.Error.ToErrorList();
        
        await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created volunteer with ID: {volunteer.Value.Id}", volunteer.Value.Id);
        
        return volunteer.Value.Id.Value;
    }
}