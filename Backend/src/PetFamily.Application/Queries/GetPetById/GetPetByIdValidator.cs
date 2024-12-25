using FluentValidation;

namespace PetFamily.Application.Queries.GetPetById;

public class GetPetByIdValidator : AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdValidator()
    {
        RuleFor(c => c.PetId).NotEmpty();
    }
}