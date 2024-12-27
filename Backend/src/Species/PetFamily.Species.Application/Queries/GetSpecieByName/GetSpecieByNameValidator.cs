using FluentValidation;

namespace PetFamily.Species.Application.Queries.GetSpecieByName;

public class GetSpecieByIdValidator : AbstractValidator<GetSpecieByNameQuery>
{
    public GetSpecieByIdValidator()
    {
        RuleFor(c => c.SpecieName).NotEmpty();
    }
}