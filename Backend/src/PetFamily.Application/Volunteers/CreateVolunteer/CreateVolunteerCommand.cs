using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;


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
