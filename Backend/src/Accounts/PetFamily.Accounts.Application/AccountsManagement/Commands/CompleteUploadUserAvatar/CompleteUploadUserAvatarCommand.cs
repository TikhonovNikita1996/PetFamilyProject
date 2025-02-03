﻿using FileService.Contracts;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Commands.CompleteUploadUserAvatar;

public record CompleteUploadUserAvatarCommand(Guid UserId,
    string FileName,
    string ContentType,
    long FileSize,
    string UploadId,
    List<PartETagInfo> Parts) : ICommand;