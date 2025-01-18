using FluentValidation;

namespace PetFamily.VolunteersRequests.Application.Commands.CreateRequest;

public class CreateRequestValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestValidator()
    {
        RuleFor(c => c.UserId).NotNull().NotEmpty();
        RuleFor(c => c.VolunteerInfo).NotNull().NotEmpty();
    }
}