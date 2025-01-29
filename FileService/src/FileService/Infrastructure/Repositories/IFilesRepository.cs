using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;

namespace FileService.Infrastructure.Repositories;

public interface IFilesRepository
{
    Task<Result<Guid, CustomError>> Add(FileData fileData, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<FileData>> Get(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
    Task<UnitResult<CustomError>> DeleteMany(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
}