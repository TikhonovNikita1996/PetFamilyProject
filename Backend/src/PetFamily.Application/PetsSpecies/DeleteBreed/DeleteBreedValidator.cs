using FluentValidation;
using PetFamily.Application.PetsSpecies.DeleteSpecie;

namespace PetFamily.Application.PetsSpecies.DeleteBreed;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(c => c.SpecieId).NotNull().NotEmpty();
        RuleFor(c => c.BreedId).NotNull().NotEmpty();
    }
}