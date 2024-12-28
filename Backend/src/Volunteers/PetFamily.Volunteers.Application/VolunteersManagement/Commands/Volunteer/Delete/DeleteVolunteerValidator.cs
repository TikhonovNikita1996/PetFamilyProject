using FluentValidation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty();
    }
}