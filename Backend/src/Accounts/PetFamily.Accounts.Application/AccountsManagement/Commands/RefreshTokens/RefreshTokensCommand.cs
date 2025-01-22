
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;

public record RefreshTokensCommand(Guid RefreshToken) : ICommand;