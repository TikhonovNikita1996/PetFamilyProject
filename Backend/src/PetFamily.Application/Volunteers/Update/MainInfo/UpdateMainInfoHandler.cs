using System.Reflection.Metadata;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.MainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMainInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, CustomError>> Handle(UpdateMainInfoCommand command,
        CancellationToken cancellationTokentoken = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationTokentoken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(command.Dto.LastName, command.Dto.FirstName, command.Dto.MiddleName).Value;
        
        var description = Description.Create(command.Dto.Description).Value;
        
        var phoneNumber = PhoneNumber.Create(command.Dto.PhoneNumber).Value;
        
        var experienceYears = WorkingExperience.Create(command.Dto.WorkingExperience).Value;
        
        volunteerResult.Value.UpdateMainInfo(fullName, description, phoneNumber, experienceYears);
        
        await _unitOfWork.SaveChanges(cancellationTokentoken);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated social networks", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}