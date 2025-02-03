using CSharpFunctionalExtensions;
using FileService.Communication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.StartUploadUserAvatar;

public class StartUploadUserAvatarHandler : ICommandHandler<StartUploadUserAvatarCommand>
{
    private readonly IFileService _fileService;
    private readonly UserManager<User> _userManager;

    public StartUploadUserAvatarHandler(IFileService fileService, UserManager<User> userManager)
    {
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<Result<StartUploadFileResponse, CustomErrorsList>> Handle(
        StartUploadUserAvatarCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = UserAvatar.Validate(
            command.FileName,
            command.ContentType,
            command.FileSize);
        
        if (validateResult.IsFailure)
            return validateResult.Error.ToErrorList();
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user == null)
        {
            return Errors.General.NotFound("user").ToErrorList();
        }

}