using CSharpFunctionalExtensions;
using FileService.Contracts;

namespace FileService.Communication;

public interface IFileService
{
    public Task<Result<IReadOnlyList<FileResponse>>> GetPresignedUrls(GetFilePresignedUrlRequest request
        ,CancellationToken cancellationToken = default);
    
    public Task<Result<FileResponse, string>> StartMultipartUpload(
        StartMultipartUploadRequest request, CancellationToken cancellationToken = default);
    
    public Task<Result<FileResponse, string>> CompleteMultipartUpload(
        CompleteMultipartRequest request, CancellationToken cancellationToken = default);
}