using FluentValidation;
using PetFamily.Species.Application.Queries.GetSpecieByName;

namespace PetFamily.Species.Application.Queries.GetBreedByName;

public class GetSpecieByIdValidator : AbstractValidator<GetBreedByNameQuery>
{
    public GetSpecieByIdValidator()
    {
        RuleFor(c => c.BreedName).NotEmpty();
    }
}