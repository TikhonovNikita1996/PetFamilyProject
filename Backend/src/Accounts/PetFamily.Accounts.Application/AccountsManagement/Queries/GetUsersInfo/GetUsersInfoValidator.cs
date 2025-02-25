using FluentValidation;

namespace PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;

public class GetUsersInfoValidator : AbstractValidator<GetUsersInfoQuery>
{
    public GetUsersInfoValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
    }
}