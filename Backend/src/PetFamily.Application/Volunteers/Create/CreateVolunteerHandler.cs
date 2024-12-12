using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid,CreateVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerHandler> logger,
        IUnitOfWork unitOfWork, 
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<Result<Guid, CustomErrorsList>> Handle(CreateVolunteerCommand createVolunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(createVolunteerCommand);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var email = Email.Create(createVolunteerCommand.Email.Value).Value;
        
        var existingVolunteer = await _volunteerRepository.GetByEmail(email);
        if (existingVolunteer.IsSuccess)
            return existingVolunteer.Error.ToErrorList();
        
        var volunterId = VolunteerId.NewId();
        var fullName = FullName.Create(createVolunteerCommand.FullName.LastName, 
            createVolunteerCommand.FullName.Name, createVolunteerCommand.FullName.MiddleName).Value;
        
        var workingExperience = WorkingExperience.Create(createVolunteerCommand.WorkingExperience.Value).Value;
        
        var description = Description.Create(createVolunteerCommand.Description.Value).Value;
        
        var phoneNumber = PhoneNumber.Create(createVolunteerCommand.PhoneNumber.Value).Value;
        
        var socialNetworks = createVolunteerCommand.SocialMediaDetails
            .Select(s => SocialMedia.Create(s.Name, s.Url));

        var resultNetworksList = new SocialMediaDetails(socialNetworks.Select(x=> x.Value).ToList());
        
        var donationInfos = createVolunteerCommand.DonationInfo
            .Select(s => DonationInfo.Create(s.Name, s.Description));

        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());
        
        var gender = Enum.Parse<GenderType>(createVolunteerCommand.Gender);
        
        var volunteer = Volunteer.Create(volunterId, fullName, email, gender,
            workingExperience, description, phoneNumber, resultDonationInfoList, resultNetworksList);
        
        if((volunteer.IsFailure))
            return volunteer.Error.ToErrorList();
        
        await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created volunteer with ID: {volunteer.Value.Id}", volunteer.Value.Id);
        
        return volunteer.Value.Id.Value;
    }
}