using CSharpFunctionalExtensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }
    
    public async Task<Result<Guid, CustomError>> Handle(CreateVolunteerCommand createVolunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(createVolunteerCommand.Email.Value);
        if(emailResult.IsFailure)
            return emailResult.Error;
        
        var existingVolunteer = await _volunteerRepository.GetByEmail(emailResult.Value);
        if (existingVolunteer.IsSuccess)
            return Errors.VolunteerValidation.AlreadyExist();
        
        var volunterId = VolunteerId.NewVolonteerId();
        var fullNameResult = FullName.Create(createVolunteerCommand.FullName.LastName, 
            createVolunteerCommand.FullName.Name, createVolunteerCommand.FullName.MiddleName);
        
        if(fullNameResult.IsFailure)
            return fullNameResult.Error;
        
        var age = createVolunteerCommand.Age;
        
        var workingExperience = createVolunteerCommand.WorkingExperience;
        
        var description = createVolunteerCommand.Description;
        
        var phoneNumberRequest = PhoneNumber.Create(createVolunteerCommand.PhoneNumber.Value);
        if(phoneNumberRequest.IsFailure)
            return fullNameResult.Error;
        
        var socialNetworks = createVolunteerCommand.SocialMediaDetails
            .Select(s => SocialMedia.Create(s.Name, s.Url));

        var resultNetworksList = new SocialMediaDetails(socialNetworks.Select(x=> x.Value).ToList());
        
        var donationInfos = createVolunteerCommand.DonationInfo
            .Select(s => DonationInfo.Create(s.Name, s.Description));

        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());
        
        var gender = Enum.Parse<GenderType>(createVolunteerCommand.Gender);
        
        var volunteer = Volunteer.Create(volunterId, fullNameResult.Value, age, emailResult.Value, gender,
            workingExperience, description, phoneNumberRequest.Value, resultDonationInfoList, resultNetworksList);
        
        if((volunteer.IsFailure))
            return volunteer.Error;
        
        await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
        
        return volunteer.Value.Id.Value;
    }
}