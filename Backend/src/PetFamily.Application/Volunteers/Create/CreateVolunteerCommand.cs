using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Create;


public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Gender,
    DateTime Birthday,
    WorkingExperienceDto WorkingExperience,
    EmailDto Email,
    PhoneNumberDto PhoneNumber,
    DescriptionDto Description,
    List<SocialMediaDetailsDto>? SocialMediaDetails,
    List<DonationInfoDto>? DonationInfo);
