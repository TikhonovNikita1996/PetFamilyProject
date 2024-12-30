using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Create;


public record CreateVolunteerCommand(
    PhoneNumberDto PhoneNumber,
    DescriptionDto Description) : ICommand;
