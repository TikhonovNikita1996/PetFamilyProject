using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;

public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ILogger<RefreshTokensHandler> _logger;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(IRefreshSessionManager refreshSessionManager, 
        ILogger<RefreshTokensHandler> logger,
        ITokenProvider tokenProvider,
        [FromKeyedServices(ProjectConstants.Context.AccountManagement)] IUnitOfWork unitOfWork)
    {
        _refreshSessionManager = refreshSessionManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<LoginResponse, CustomErrorsList>> Handle(RefreshTokensCommand command,
        CancellationToken cancellationToken)
    {
        var oldRefreshSession = await _refreshSessionManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);
        
        if (oldRefreshSession.IsFailure)
            return oldRefreshSession.Error.ToErrorList();

        if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
            return Errors.Tokens.ExpiredToken().ToErrorList();
        
        _refreshSessionManager.Delete(oldRefreshSession.Value);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        var accessToken = await _tokenProvider.GenerateAccessToken(oldRefreshSession.Value.User);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(oldRefreshSession.Value.User,
            accessToken.Jti, cancellationToken);

        return new LoginResponse(accessToken.AccessToken, refreshToken,
            oldRefreshSession.Value.UserId, oldRefreshSession.Value.User.Email!);
    }
}