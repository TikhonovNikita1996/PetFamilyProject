using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.CompleteUploadUserAvatar;

public class CompleteUploadUserAvatarHandler : ICommandHandler<CompleteUploadUserAvatarCommand>
{
    private readonly IFileService _fileService;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteUploadUserAvatarHandler(IFileService fileService,
        UserManager<User> userManager,
        [FromKeyedServices(ProjectConstants.Context.AccountManagement)] IUnitOfWork unitOfWork)
    {
        _fileService = fileService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<CustomErrorsList>> Handle(
        CompleteUploadUserAvatarCommand command,
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
            return Errors.General.NotFound("user").ToErrorList();
        
        var completeRequest = new CompleteMultipartRequest(command.UploadId, command.Parts);
        
        var result = await _fileService.CompleteMultipartUpload(completeRequest, cancellationToken);
        
        if (result.IsFailure)
            return Errors.General.Failure(result.Error).ToErrorList();

        var avatar = new UserAvatar(result.Value.FileId);
        
        user.Photo = avatar;

        await _unitOfWork.SaveChanges(cancellationToken);
        
        return UnitResult.Success<CustomErrorsList>();
    }
}