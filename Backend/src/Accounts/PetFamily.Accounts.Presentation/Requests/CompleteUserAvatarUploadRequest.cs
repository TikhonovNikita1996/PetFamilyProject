using FileService.Contracts;

namespace PetFamily.Accounts.Presentation.Requests;

public record CompleteUserAvatarUploadRequest (Guid UserId,
    string FileName,
    string ContentType,
    int Size,
    string UploadId,
    List<PartETagInfo> Parts);