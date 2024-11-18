using PetFamily.Application.Dtos;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;


public record CreateVolunteerCommand(
                FullNameDto FullName,
                int Age,
                string Gender,
                DateTime Birthday,
                int WorkingExperience,
                EmailDto Email,
                PhoneNumberDto PhoneNumber,
                string Description,
                List<SocialMediaDetailsDto>? SocialMediaDetails,
                List<DonationInfoDto>? DonationInfo);
