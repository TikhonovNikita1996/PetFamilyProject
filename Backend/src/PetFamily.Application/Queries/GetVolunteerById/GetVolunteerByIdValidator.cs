using FluentValidation;
using PetFamily.Application.Queries.GetSpecieById;

namespace PetFamily.Application.Queries.GetVolunteerById;

public class GetVolunteerByIdValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty();
    }
}