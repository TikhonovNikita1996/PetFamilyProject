using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, CustomError>> Handle(CreateVolunteerCommand createVolunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var email = Email.Create(createVolunteerCommand.Email.Value).Value;
        
        var existingVolunteer = await _volunteerRepository.GetByEmail(email);
        if (existingVolunteer.IsSuccess)
            return Errors.VolunteerValidation.AlreadyExist();
        
        var volunterId = VolunteerId.NewVolonteerId();
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
            return volunteer.Error;
        
        await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Created volunteer with ID: {volunteer.Value.Id}", volunteer.Value.Id);
        
        return volunteer.Value.Id.Value;
    }
}