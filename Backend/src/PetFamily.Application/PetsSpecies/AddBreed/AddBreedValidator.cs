using FluentValidation;

namespace PetFamily.Application.PetsSpecies.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name).NotNull().NotEmpty();
    }
}