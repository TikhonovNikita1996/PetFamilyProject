using FluentValidation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllSubmuttedRequests;

public class GetAllSubmittedRequestsValidator : AbstractValidator<GetAllSubmittedRequestsQuery>
{
    public GetAllSubmittedRequestsValidator()
    {
        RuleFor(c => c.Page).NotEmpty().GreaterThan(0);
        RuleFor(c => c.PageSize).NotEmpty().GreaterThan(0);
    }
}