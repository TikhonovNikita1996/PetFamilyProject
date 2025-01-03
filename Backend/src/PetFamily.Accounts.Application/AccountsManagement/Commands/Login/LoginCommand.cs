
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;