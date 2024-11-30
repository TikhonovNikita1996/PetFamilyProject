using System.Reflection.Metadata;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.MainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, CustomError>> Handle(UpdateMainInfoRequest request,
        CancellationToken token = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, token);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(request.Dto.LastName, request.Dto.FirstName, request.Dto.MiddleName).Value;
        
        var description = Description.Create(request.Dto.Description).Value;
        
        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;
        
        var experienceYears = WorkingExperience.Create(request.Dto.WorkingExperience).Value;
        
        volunteerResult.Value.UpdateMainInfo(fullName, description, phoneNumber, experienceYears);
        
        await _volunteerRepository.Save(volunteerResult.Value, token);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated social networks", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}