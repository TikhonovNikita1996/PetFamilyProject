using FluentValidation;

namespace PetFamily.Application.Pets.Breed;

public class CreateBreedValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedValidator()
    {
        RuleFor(c => c.Name).NotNull();
    }
}