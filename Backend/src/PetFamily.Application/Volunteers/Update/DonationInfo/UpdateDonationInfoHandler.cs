using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.DonationInfo;

public class UpdateDonationInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDonationInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, CustomError>> Handle(UpdateDonationInfoCommand command,
        CancellationToken token = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(command.VolonteerId, token);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var newDonationInfos = command.UpdateDonationInfoDto.DonationInfos.Select(sn =>
            Domain.Entities.Volunteer.ValueObjects.DonationInfo.Create(sn.Name, sn.Description).Value).ToList();
        
        volunteerResult.Value.UpdateDonationInfo(new Domain.Entities.Others.DonationInfoList(newDonationInfos));

        await _unitOfWork.SaveChanges(token);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated donation info", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}