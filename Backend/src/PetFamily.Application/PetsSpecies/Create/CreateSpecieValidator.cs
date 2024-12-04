using FluentValidation;

namespace PetFamily.Application.PetsSpecies.Create;

public class CreateSpeciesValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.Name).NotNull().NotEmpty();
    }
}