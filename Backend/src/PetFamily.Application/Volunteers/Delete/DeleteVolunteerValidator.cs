using FluentValidation;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty();
    }
}