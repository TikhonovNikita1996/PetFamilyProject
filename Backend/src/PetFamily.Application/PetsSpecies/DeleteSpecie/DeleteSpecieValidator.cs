using FluentValidation;
using PetFamily.Application.PetsSpecies.Create;

namespace PetFamily.Application.PetsSpecies.DeleteSpecie;

public class DeleteSpeciesValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpeciesValidator()
    {
        RuleFor(c => c.SpecieId).NotNull().NotEmpty();
    }
}