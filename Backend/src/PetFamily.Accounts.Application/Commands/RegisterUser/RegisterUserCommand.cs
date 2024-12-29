
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string UserName ,string Password) : ICommand;