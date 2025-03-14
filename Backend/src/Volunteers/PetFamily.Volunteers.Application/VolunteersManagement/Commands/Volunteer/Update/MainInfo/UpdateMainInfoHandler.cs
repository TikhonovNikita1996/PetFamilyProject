﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.MainInfo;

public class UpdateMainInfoHandler : ICommandHandler<Guid,UpdateMainInfoCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoCommand> _validator;

    public UpdateMainInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger, 
        [FromKeyedServices(ProjectConstants.Context.VolunteerManagement)] IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullName = FullName.Create(command.Dto.LastName, command.Dto.FirstName, command.Dto.MiddleName).Value;
        
        var description = Description.Create(command.Dto.Description).Value;
        
        var phoneNumber = PhoneNumber.Create(command.Dto.PhoneNumber).Value;
        
        var experienceYears = WorkingExperience.Create(command.Dto.WorkingExperience).Value;
        
        volunteerResult.Value.UpdateMainInfo(fullName, description, phoneNumber, experienceYears);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated social networks", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}