using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(UserManager<User> userManager, 
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<UnitResult<CustomErrorsList>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if(existingUser != null)
            return Errors.General.AlreadyExists().ToErrorList();

        // var user = new User
        // {
        //     Email = command.Email,
        //     UserName = command.UserName
        // };
        //
        // var result = await _userManager.CreateAsync(user, command.Password);
        // if (!result.Succeeded)
        // {
        //     var errors = result.Errors.Select(e => CustomError.Failure(e.Code, e.Description)).ToList();
        //     return new CustomErrorsList(errors);
        // }
        // _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);
        return UnitResult.Success<CustomErrorsList>();
    }
}