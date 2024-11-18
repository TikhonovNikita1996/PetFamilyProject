using FluentValidation;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.FullName).NotNull().NotEmpty();
        RuleFor(c => c.Age).NotNull().NotEqual(0);
        RuleFor(c => c.Gender).NotNull().Must(g => g is "Male" or "Female");
        
    }
}