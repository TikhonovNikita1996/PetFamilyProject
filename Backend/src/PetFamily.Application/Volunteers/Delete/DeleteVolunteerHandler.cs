using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, CustomError>> Handle(DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var result = await _volunteerRepository.Delete(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer with Id: {id} was deleted", volunteerResult.Value.Id);
        
        return result;
    }
}