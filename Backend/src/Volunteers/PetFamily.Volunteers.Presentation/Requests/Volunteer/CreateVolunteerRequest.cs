using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Gender,
    WorkingExperienceDto WorkingExperience,
    EmailDto Email,
    PhoneNumberDto PhoneNumber,
    DescriptionDto Description,
    List<SocialMediaDetailsDto>? SocialMediaDetails,
    List<DonationInfoDto>? DonationInfo);