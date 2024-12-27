using FluentValidation;

namespace PetFamily.Species.Application.Commands.DeleteSpecie;

public class DeleteSpeciesValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpeciesValidator()
    {
        RuleFor(c => c.SpecieId).NotNull().NotEmpty();
    }
}