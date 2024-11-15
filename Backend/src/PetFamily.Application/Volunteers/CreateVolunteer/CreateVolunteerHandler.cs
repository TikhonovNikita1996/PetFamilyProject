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
    
    public async Task<CustomResult<Guid>> Handle(CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var volunterId = VolunteerId.NewVolonteerId();
        var fullName = FullName.Create(createVolunteerRequest.FullName.LastName, 
            createVolunteerRequest.FullName.Name, createVolunteerRequest.FullName.MiddleName).Value;
        
        var age = createVolunteerRequest.Age;
        
        var email = createVolunteerRequest.Email;

        var workingExperience = createVolunteerRequest.WorkingExperience;
        
        var description = createVolunteerRequest.Description;
        
        var phoneNumber = createVolunteerRequest.PhoneNumber;
        
        var socialNetworks = createVolunteerRequest.SocialMediaDetails
            .Select(s => SocialMedia.Create(s.Name, s.Url));

        var resultNetworksList = new SocialMediaDetails(socialNetworks.Select(x=> x.Value).ToList());
        
        var donationInfos = createVolunteerRequest.DonationInfo
            .Select(s => DonationInfo.Create(s.Name, s.Description));

        var resultDonationInfoList = new DonationInfoList(donationInfos.Select(x=> x.Value).ToList());
        
        var gender = Enum.Parse<GenderType>(createVolunteerRequest.Gender);
        
        var volunteer = new Volunteer(volunterId, fullName, age, email, gender,
            workingExperience, description, phoneNumber, resultDonationInfoList, resultNetworksList);
        
        await _volunteerRepository.AddAsync(volunteer, cancellationToken);
        
        return volunteer.Id.Value;
    }
}