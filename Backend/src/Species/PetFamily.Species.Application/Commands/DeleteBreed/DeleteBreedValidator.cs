using FluentValidation;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(c => c.SpecieId).NotNull().NotEmpty();
        RuleFor(c => c.BreedId).NotNull().NotEmpty();
    }
}