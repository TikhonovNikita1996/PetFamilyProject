﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using Microsoft.Extensions.Options;
using PetFamily.Core.Options;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }
    
    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var roleClaims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name ?? string.Empty));
        
        Claim[] claims = 
        [
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Email, user.Email)
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
        
        return token;
    }
}