using FluentValidation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.StartUploadPhotosToPet;

public class StartUploadPhotosToPetValidator : AbstractValidator<StartUploadPhotosToPetCommand>
{
    public StartUploadPhotosToPetValidator()
    {
        RuleFor(c => c.PetId).NotNull().NotEmpty();
        RuleFor(c => c.Files).NotNull().NotEmpty();
        RuleFor(c => c.VolunteerId).NotNull().NotEmpty();
    }
}