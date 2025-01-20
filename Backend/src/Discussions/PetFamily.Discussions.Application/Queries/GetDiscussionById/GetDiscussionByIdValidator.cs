using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionById;

public class GetDiscussionByIdValidator : AbstractValidator<GetDiscussionByIdQuery>
{
    public GetDiscussionByIdValidator()
    {
        RuleFor(c => c.DiscussionId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}