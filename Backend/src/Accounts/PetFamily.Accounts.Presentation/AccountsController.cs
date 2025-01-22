using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.AccountsManagement.Commands.Login;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RefreshTokens;
using PetFamily.Accounts.Application.AccountsManagement.Commands.RegisterUser;
using PetFamily.Accounts.Application.AccountsManagement.Commands.UpdateSocialNetworks;
using PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Presentation;

public class AccountsController : BaseApiController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken) 
    {
        var command = new RegisterUserCommand(request.FullNameDto,request.Email,
            request.UserName, request.Password);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken) 
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString(),
            new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(30),
                IsEssential = true
            });
        
        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken) 
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }
        
        var command = new RefreshTokensCommand(Guid.Parse(refreshToken!));
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString(),
            new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(30),
                IsEssential = true
            });
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Accounts.Update)]
    [HttpPut("{userId:guid}/social-networks")]
    public async Task<ActionResult> ChangePetsPosition(
        [FromRoute] Guid userId,
        [FromBody] UpdateSocialNetworksRequest request,
        [FromServices] UpdateSocialNetworksHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSocialNetworksCommand(userId, request.SocialMediaDetails);
            
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse(); 
            
        return Ok(result.Value);
    }
    
    [HttpGet("/userInfo-by-id")]
    public async Task<ActionResult> GetPetById(
        [FromServices] GetUsersInfoHandler handler,
        [FromQuery] GetUsersInfoRequest request, 
        CancellationToken cancellationToken)
    {
        var query = new GetUsersInfoQuery(request.UserId);
        var result = await handler.Handle(query, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
}