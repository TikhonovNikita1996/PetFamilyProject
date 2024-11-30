using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.DonationInfo;

public class UpdateDonationInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateDonationInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, CustomError>> Handle(UpdateDonationInfoRequest request,
        CancellationToken token = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolonteerId, token);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var newDonationInfos = request.UpdateDonationInfoDto.DonationInfos.Select(sn =>
            Domain.Entities.Volunteer.ValueObjects.DonationInfo.Create(sn.Name, sn.Description).Value).ToList();
        
        volunteerResult.Value.UpdateDonationInfo(new Domain.Entities.Others.DonationInfoList(newDonationInfos));
        
        await _volunteerRepository.Save(volunteerResult.Value, token);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated donation info", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}