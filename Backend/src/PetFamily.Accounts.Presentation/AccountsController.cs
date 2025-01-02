using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Framework;

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
        return Ok(result.Value);
    }
}