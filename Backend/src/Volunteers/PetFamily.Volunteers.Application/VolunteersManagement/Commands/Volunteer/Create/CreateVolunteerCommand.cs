using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Create;


public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Gender,
    WorkingExperienceDto WorkingExperience,
    EmailDto Email,
    PhoneNumberDto PhoneNumber,
    DescriptionDto Description,
    List<SocialMediaDetailsDto>? SocialMediaDetails,
    List<DonationInfoDto>? DonationInfo) : ICommand;
