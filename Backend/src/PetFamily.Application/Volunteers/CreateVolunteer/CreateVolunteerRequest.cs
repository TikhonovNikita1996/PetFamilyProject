using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    int Age,
    string Gender,
    DateTime Birthday,
    int WorkingExperience,
    string Email,
    string PhoneNumber,
    string Description,
    List<SocialMediaDetailsDto>? SocialMediaDetails,
    List<DonationInfoDto>? DonationInfo);
