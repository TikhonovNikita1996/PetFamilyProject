using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.DonationInfo;

public class UpdateDonationInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateDonationInfoCommand> _validator;

    public UpdateDonationInfoHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<UpdateDonationInfoCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(UpdateDonationInfoCommand command,
        CancellationToken token = default)
    {
        
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolonteerId, token);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var newDonationInfos = command.UpdateDonationInfoDto.DonationInfos.Select(sn =>
            Domain.Entities.Volunteer.ValueObjects.DonationInfo.Create(sn.Name, sn.Description).Value).ToList();
        
        volunteerResult.Value.UpdateDonationInfo(new Domain.Entities.Others.DonationInfoList(newDonationInfos));

        await _unitOfWork.SaveChanges(token);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated donation info", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}