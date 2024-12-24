using FluentValidation;

namespace PetFamily.Application.Queries.GetSpecieById;

public class GetSpecieByIdValidator : AbstractValidator<GetSpecieByIdQuery>
{
    public GetSpecieByIdValidator()
    {
        RuleFor(c => c.SpecieId).NotEmpty();
    }
}