using FluentValidation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;

public class GetPetByIdValidator : AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdValidator()
    {
        RuleFor(c => c.PetId).NotEmpty();
    }
}