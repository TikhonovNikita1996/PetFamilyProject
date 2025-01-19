using FluentValidation;
using PetFamily.VolunteersRequests.Application.Commands.CreateRequest;

namespace PetFamily.VolunteersRequests.Application.Commands.TakeInReview;

public class TakeInReviewValidator : AbstractValidator<TakeInReviewCommand>
{
    public TakeInReviewValidator()
    {
        RuleFor(c => c.AdminId).NotNull().NotEmpty();
        RuleFor(c => c.RequestId).NotNull().NotEmpty();
    }
}