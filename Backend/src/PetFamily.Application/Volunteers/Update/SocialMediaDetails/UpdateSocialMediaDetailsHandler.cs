using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.SocialMediaDetails;

public class UpdateSocialMediaDetailsHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateSocialMediaDetailsHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, CustomError>> Handle(UpdateSocialMediaDetailsRequest request,
        CancellationToken token = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolonteerId, token);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var newSocialNetworks = request.UpdateSocialNetworksDto.SocialNetworks.Select(sn =>
            SocialMedia.Create(sn.Name, sn.Url).Value).ToList();
        
        volunteerResult.Value.UpdateSocialMediaDetails(new Domain.Entities.Others.SocialMediaDetails(newSocialNetworks));
        
        await _volunteerRepository.Save(volunteerResult.Value, token);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated social networks", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}