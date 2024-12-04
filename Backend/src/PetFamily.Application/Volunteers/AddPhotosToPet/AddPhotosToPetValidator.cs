using FluentValidation;

namespace PetFamily.Application.Volunteers.AddPhotosToPet;

public class AddPhotosToPetValidator : AbstractValidator<AddPhotosToPetCommand>
{
    public AddPhotosToPetValidator()
    {
        RuleFor(c => c.PetId).NotNull().NotEmpty();
        RuleFor(c => c.Files).NotNull().NotEmpty();
        RuleFor(c => c.VolunteerId).NotNull().NotEmpty();
    }
}