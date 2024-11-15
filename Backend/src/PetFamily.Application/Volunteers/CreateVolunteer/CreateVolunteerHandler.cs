using PetFamily.Application.Dtos;
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
    
    public async Task<CustomResult<Guid>> Handle(CreateVolunteerCommand createVolunteerRCommand,
        CancellationToken cancellationToken = default)
    {
        var volunterId = VolunteerId.NewVolonteerId();
        var fullName = FullName.Create(createVolunteerRCommand.FullName.LastName, 
            createVolunteerRCommand.FullName.Name, createVolunteerRCommand.FullName.MiddleName).Value;
        
        var age = createVolunteerRCommand.Age;
        
        var email = createVolunteerRCommand.Email;

        var workingExperience = createVolunteerRCommand.WorkingExperience;
        
        var description = createVolunteerRCommand.Description;
        
        var phoneNumber = createVolunteerRCommand.PhoneNumber;
        
        var socialNetworks = createVolunteerRCommand.SocialMediaDetails
            .Select(s => SocialMedia.Create(s.Name, s.Url));

        var resultNetworksList = new SocialMediaDetails(socialNetworks.Select(x=> x.Value).ToList());
        
        var donationInfos = createVolunteerRCommand.DonationInfo
            .Select(s => DonationInfo.Create(s.Name, s.Description));

        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());
        
        var gender = Enum.Parse<GenderType>(createVolunteerRCommand.Gender);
        
        var volunteer = new Volunteer(volunterId, fullName, age, email, gender,
            workingExperience, description, phoneNumber, resultDonationInfoList, resultNetworksList);
        
        await _volunteerRepository.AddAsync(volunteer, cancellationToken);
        
        return volunteer.Id.Value;
    }
}