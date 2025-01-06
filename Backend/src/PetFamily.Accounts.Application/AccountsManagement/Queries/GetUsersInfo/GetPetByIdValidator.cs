using FluentValidation;

namespace PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;

public class GetPetByIdValidator : AbstractValidator<GetUsersInfoQuery>
{
    public GetPetByIdValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
    }
}