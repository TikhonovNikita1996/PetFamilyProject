using FluentValidation;

namespace PetFamily.Species.Application.Commands.Create;

public class CreateSpeciesValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.Name).NotNull().NotEmpty();
    }
}