using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IFileProvider
{
    Task<Result<string, CustomError>> UploadFile(FileData fileData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, CustomError>> DeleteFile(FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, CustomError>> GetFile(FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);
}