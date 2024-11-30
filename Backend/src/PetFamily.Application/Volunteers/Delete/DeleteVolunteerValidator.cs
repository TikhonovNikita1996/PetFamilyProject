using FluentValidation;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(v => v.VolunteerId).NotEmpty();
    }
}