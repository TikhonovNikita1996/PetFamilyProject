using FluentValidation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByUserId;

public class GetAllRequestsByUserIdValidator : AbstractValidator<GetAllRequestsByUserIdQuery>
{
    public GetAllRequestsByUserIdValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Page).NotEmpty().GreaterThan(0);
        RuleFor(c => c.PageSize).NotEmpty().GreaterThan(0);
    }
}