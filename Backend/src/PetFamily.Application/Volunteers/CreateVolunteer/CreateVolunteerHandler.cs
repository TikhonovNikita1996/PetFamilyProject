using CSharpFunctionalExtensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;

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
        var volunterId = VolunteerId.NewVolonteerId();
        var fullName = FullName.Create(createVolunteerCommand.FullName.LastName, 
            createVolunteerCommand.FullName.Name, createVolunteerCommand.FullName.MiddleName).Value;
        
        var age = createVolunteerCommand.Age;
        
        var email = createVolunteerCommand.Email;

        var workingExperience = createVolunteerCommand.WorkingExperience;
        
        var description = createVolunteerCommand.Description;
        
        var phoneNumber = createVolunteerCommand.PhoneNumber;
        
        var socialNetworks = createVolunteerCommand.SocialMediaDetails
            .Select(s => SocialMedia.Create(s.Name, s.Url));

        var resultNetworksList = new SocialMediaDetails(socialNetworks.Select(x=> x.Value).ToList());
        
        var donationInfos = createVolunteerCommand.DonationInfo
            .Select(s => DonationInfo.Create(s.Name, s.Description));

        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());
        
        var gender = Enum.Parse<GenderType>(createVolunteerCommand.Gender);
        
        var volunteer = Volunteer.Create(volunterId, fullName, age, email, gender,
            workingExperience, description, phoneNumber, resultDonationInfoList, resultNetworksList);
        
        if((volunteer.IsFailure))
            return volunteer.Error;
        
        await _volunteerRepository.AddAsync(volunteer.Value, cancellationToken);
        
        return volunteer.Value.Id.Value;
    }
}