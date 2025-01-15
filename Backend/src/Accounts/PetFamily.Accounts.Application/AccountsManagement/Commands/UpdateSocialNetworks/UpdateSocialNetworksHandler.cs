using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpdateSocialNetworksHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSocialNetworksCommand> _validator;

    public UpdateSocialNetworksHandler(UserManager<User> userManager, 
        ILogger<UpdateSocialNetworksHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.AccountManagement)] IUnitOfWork unitOfWork,
        IValidator<UpdateSocialNetworksCommand> _validator)
    {
        _userManager = userManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
        this._validator = _validator;
    }
    
    public async Task <Result<Guid, CustomErrorsList>> Handle(UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);

        if (user is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var socialNetworks = command.SocialMediaDetailsDtos
            .Select(x => SocialMedia.Create(x.Name, x.Url).Value).ToList();
        
        user.UpdateSocialMediaDetails(socialNetworks);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.Log(LogLevel.Information, "Users {user.Id} social links was updated", user.Id);

        return user.Id;
    }
}