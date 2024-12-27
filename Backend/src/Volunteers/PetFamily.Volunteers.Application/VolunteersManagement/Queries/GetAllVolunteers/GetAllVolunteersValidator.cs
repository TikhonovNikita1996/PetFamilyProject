using FluentValidation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetAllVolunteers;

public class GetAllVolunteersValidator : AbstractValidator<GetAllVolunteersQuery>
{
    public GetAllVolunteersValidator()
    {
        RuleFor(c => c.Page).NotEmpty();
        RuleFor(c => c.PageSize).NotEmpty();
    }
}