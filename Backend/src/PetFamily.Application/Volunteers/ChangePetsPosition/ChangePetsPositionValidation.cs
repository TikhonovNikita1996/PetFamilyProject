using FluentValidation;

namespace PetFamily.Application.Volunteers.ChangePetsPosition;

public class ChangePetsPositionValidation : AbstractValidator<ChangePetsPositionCommand>
{
    public ChangePetsPositionValidation()
    {
        RuleFor(c => c.VolunteerId).NotNull().NotEmpty();
        RuleFor(c => c.PetId).NotNull().NotEmpty();
        RuleFor(c => c.NewPosition).NotNull().Must(g => g >= 0 );
    }
}