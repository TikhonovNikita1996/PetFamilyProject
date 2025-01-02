
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(FullNameDto FullNameDto,
    string Email, string UserName ,string Password) : ICommand;