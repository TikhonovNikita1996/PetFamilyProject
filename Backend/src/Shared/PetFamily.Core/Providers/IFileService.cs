using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Core.Providers;

public interface IFileService
{
    public Task<Result<IReadOnlyList<FilePath>, CustomError>> UploadFilesAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    public Task<Result<string, CustomError>> UploadFileAsync(
        FileData fileData,
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<CustomError>> DeleteFileAsync(FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, CustomError>> GetFileAsync(FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);
}