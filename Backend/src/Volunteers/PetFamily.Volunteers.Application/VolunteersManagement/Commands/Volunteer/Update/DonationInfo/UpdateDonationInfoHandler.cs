using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.MainInfo;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.DonationInfo;

public class UpdateDonationInfoHandler : ICommandHandler<Guid,UpdateDonationInfoCommand>
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
            Domain.Volunteer.ValueObjects.DonationInfo.Create(sn.Name, sn.Description).Value).ToList();
        
        volunteerResult.Value.UpdateDonationInfo(new DonationInfoList(newDonationInfos));

        await _unitOfWork.SaveChanges(token);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated donation info", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}