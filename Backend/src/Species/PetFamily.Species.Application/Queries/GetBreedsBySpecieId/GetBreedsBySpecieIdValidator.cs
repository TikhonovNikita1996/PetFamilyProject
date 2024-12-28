using FluentValidation;

namespace PetFamily.Species.Application.Queries.GetBreedsBySpecieId;

public class GetBreedsBySpecieIdValidator : AbstractValidator<GetBreedsBySpecieIdQuery>
{
    public GetBreedsBySpecieIdValidator()
    {
        RuleFor(c => c.SpecieId).NotEmpty();
        RuleFor(c => c.Page).NotEmpty();
        RuleFor(c => c.PageSize).NotEmpty();
    }
}