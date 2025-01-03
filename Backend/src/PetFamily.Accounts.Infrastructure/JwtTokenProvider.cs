using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using Microsoft.Extensions.Options;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;
using PetFamily.Core.Options;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly WriteAccountsDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options, WriteAccountsDbContext dbContext)
    {
        _dbContext = dbContext;
        _jwtOptions = options.Value;
    }
    
    public async Task <JwtTokenResult> GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var roleClaims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name ?? string.Empty));

        var jti = Guid.NewGuid();
        
        Claim[] claims = 
        [
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Email, user.Email!)
        ];
         
        claims = claims.Concat(roleClaims).ToArray();
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpirationTime)),
            signingCredentials: signingCredentials,
            claims: claims
        );
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return new JwtTokenResult(token, jti);
    }
    
    public async Task<Guid> GenerateRefreshToken(User user, Guid jti,
        CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(30),
            UserId = user.Id,
            RefreshToken = Guid.NewGuid(),
            Jti = jti
        };
        
        _dbContext.RefreshSessions.Add(refreshSession);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>, CustomErrorsList>> GetUserClaimFromJwtToken(string jwtToken,
        CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var validationParams = TokenValidationParametersFactory.Create(_jwtOptions, false);
        
        var result =  await jwtHandler.ValidateTokenAsync(jwtToken,validationParams);

        if (result.IsValid == false)
            return Errors.Tokens.InvalidToken().ToErrorList();

        return result.ClaimsIdentity.Claims.ToList();
    }
}