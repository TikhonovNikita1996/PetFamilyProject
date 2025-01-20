using FluentValidation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByAdminId;

public class GetAllRequestsByAdminIdValidator : AbstractValidator<GetAllRequestsByAdminIdQuery>
{
    public GetAllRequestsByAdminIdValidator()
    {
        RuleFor(c => c.AdminId).NotEmpty();
        RuleFor(c => c.Page).NotEmpty().GreaterThan(0);
        RuleFor(c => c.PageSize).NotEmpty().GreaterThan(0);
    }
}