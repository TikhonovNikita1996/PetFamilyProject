using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.StartUploadUserAvatar;

public record StartUploadUserAvatarCommand(Guid UserId,
    string FileName,
    string ContentType,
    int FileSize) : ICommand;