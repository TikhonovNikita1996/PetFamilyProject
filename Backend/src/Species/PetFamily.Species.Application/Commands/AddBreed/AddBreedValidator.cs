using FluentValidation;

namespace PetFamily.Species.Application.Commands.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name).NotNull().NotEmpty();
    }
}