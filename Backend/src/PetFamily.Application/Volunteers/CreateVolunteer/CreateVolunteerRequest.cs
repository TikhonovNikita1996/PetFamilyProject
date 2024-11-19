using System.Data;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

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