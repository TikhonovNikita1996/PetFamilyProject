using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<FilePath>, CustomError>> UploadFilesAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    public Task<Result<string, CustomError>> UploadFileAsync(
        FileData fileData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, CustomError>> DeleteFileAsync(FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, CustomError>> GetFileAsync(FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);
}