using FluentValidation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetsWithFiltersAndPagination;

public class GetPetsWithPaginationAndFiltersValidator : AbstractValidator<GetPetsWithPaginationAndFiltersQuery>
{
    public GetPetsWithPaginationAndFiltersValidator()
    {
        RuleFor(c => c.Page).NotEmpty().GreaterThan(0);
        RuleFor(c => c.PageSize).NotEmpty().GreaterThan(0);
    }
}