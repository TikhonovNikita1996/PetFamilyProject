using FluentValidation;

namespace PetFamily.Application.Pets.Species;

public class CreateBreedValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateBreedValidator()
    {
        RuleFor(c => c.Name).NotNull();
    }
}