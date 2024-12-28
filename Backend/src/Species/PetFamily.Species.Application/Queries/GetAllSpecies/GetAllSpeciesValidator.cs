using FluentValidation;

namespace PetFamily.Species.Application.Queries.GetAllSpecies;

public class GetAllSpeciesValidator : AbstractValidator<GetAllSpeciesQuery>
{
    public GetAllSpeciesValidator()
    {
        RuleFor(c => c.Page).NotEmpty();
        RuleFor(c => c.PageSize).NotEmpty();
    }
}