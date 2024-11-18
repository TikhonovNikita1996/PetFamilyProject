using System.Data;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    int Age,
    string Gender,
    DateTime Birthday,
    int WorkingExperience,
    EmailDto Email,
    string PhoneNumber,
    string Description,
    List<SocialMediaDetailsDto>? SocialMediaDetails,
    List<DonationInfoDto>? DonationInfo);