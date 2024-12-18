using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Volunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Gender,
    DateTime Birthday,
    WorkingExperienceDto WorkingExperience,
    EmailDto Email,
    PhoneNumberDto PhoneNumber,
    DescriptionDto Description,
    List<SocialMediaDetailsDto>? SocialMediaDetails,
    List<DonationInfoDto>? DonationInfo);