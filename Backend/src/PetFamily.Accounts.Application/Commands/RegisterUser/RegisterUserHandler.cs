using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(UserManager<User> userManager, 
        ILogger<RegisterUserHandler> logger,
        RoleManager<Role> roleManager,
        IAccountManager accountManager,
        [FromKeyedServices(ProjectConstants.Context.AccountManagement)] IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<CustomErrorsList>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var existingUser = await _userManager.FindByEmailAsync(command.Email);
            if(existingUser != null)
                return Errors.General.AlreadyExists().ToErrorList();

            var participantRole = await _roleManager.FindByNameAsync(ParticipantAccount.RoleName)
                                  ?? throw new ApplicationException("Participant role is not found");
        
            var usersName = FullName.Create(command.FullNameDto.LastName, command.FullNameDto.Name
                ,command.FullNameDto.MiddleName!).Value;
        
            var user = User.CreateParticipant(command.Email, command.UserName, usersName, participantRole);
        
            var result = await _userManager.CreateAsync(user, command.Password);
        
            var participantAccount = new ParticipantAccount(user);
            await _accountManager.CreateParticipantAccount(participantAccount);
        
            user.ParticipantAccount = participantAccount;
        
            await _userManager.UpdateAsync(user);
            
            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("User {username} was registered", user.UserName);
            return UnitResult.Success<CustomErrorsList>();
        }
        
        catch (Exception e)
        {
            _logger.LogError("Failed to register user {username}", command.UserName);
            transaction.Rollback();

            return CustomError.Failure("could.not.register.user", e.Message).ToErrorList();
        }
    }
}