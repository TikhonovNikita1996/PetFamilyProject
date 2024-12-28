using FluentValidation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetVolunteerById;

public class GetVolunteerByIdValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty();
    }
}