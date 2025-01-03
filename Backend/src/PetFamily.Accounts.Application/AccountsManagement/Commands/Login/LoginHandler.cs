using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.Login;

public class LoginHandler : ICommandHandler<LoginResponse, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginHandler> _logger;
    private readonly ITokenProvider _tokenProvider;

    public LoginHandler(UserManager<User> userManager, 
        ILogger<LoginHandler> logger,
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<LoginResponse, CustomErrorsList>> Handle(LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Errors.General.NotFound("").ToErrorList();

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        
        if (!passwordConfirmed)
            return Errors.User.InvalidCredentials().ToErrorList();
        
        var accessToken = await _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, accessToken.Jti, cancellationToken);
       
        _logger.LogInformation("User: {userName} logged in.", user.UserName);
        
        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}