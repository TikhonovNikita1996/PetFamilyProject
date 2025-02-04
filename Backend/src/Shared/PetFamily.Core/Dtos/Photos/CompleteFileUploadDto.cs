using FileService.Contracts;

namespace PetFamily.Core.Dtos.Photos;

public record CompleteFileUploadDto(string FileName,
    string ContentType,
    int FileSize,
    string UploadId,
    List<PartETagInfo> Parts);