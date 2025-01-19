using FluentValidation;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.TakeInReview;

public class TakeInReviewValidator : AbstractValidator<TakeInReviewCommand>
{
    public TakeInReviewValidator()
    {
        RuleFor(c => c.AdminId).NotNull().NotEmpty();
        RuleFor(c => c.RequestId).NotNull().NotEmpty();
    }
}