using FluentValidation;

namespace PetFamily.Species.Application.Queries.GetSpecieById;

public class GetSpecieByIdValidator : AbstractValidator<GetSpecieByIdQuery>
{
    public GetSpecieByIdValidator()
    {
        RuleFor(c => c.SpecieId).NotEmpty();
    }
}