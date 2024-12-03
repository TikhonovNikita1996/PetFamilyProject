using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.SocialMediaDetails;

public class UpdateSocialMediaDetailsHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSocialMediaDetailsHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger, IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, CustomError>> Handle(UpdateSocialMediaDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(command.VolonteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var newSocialNetworks = command.UpdateSocialNetworksDto.SocialNetworks.Select(sn =>
            SocialMedia.Create(sn.Name, sn.Url).Value).ToList();
        
        volunteerResult.Value.UpdateSocialMediaDetails(new Domain.Entities.Others.SocialMediaDetails(newSocialNetworks));
        
        await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated social networks", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}