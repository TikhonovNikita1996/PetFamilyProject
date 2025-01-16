using System.Security.Claims;
using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    Task <JwtTokenResult> GenerateAccessToken(User user);
    Task <Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken);

    public Task<Result<IReadOnlyList<Claim>, CustomErrorsList>> GetUserClaimFromJwtToken(string jwtToken,
        CancellationToken cancellationToken);
}