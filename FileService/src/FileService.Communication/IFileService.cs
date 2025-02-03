using CSharpFunctionalExtensions;
using FileService.Contracts;

namespace FileService.Communication;

public interface IFileService
{
    public Task<Result<IReadOnlyList<FileResponse>>> GetPresignedUrls(GetFilePresignedUrlRequest request
        , CancellationToken cancellationToken = default);
}