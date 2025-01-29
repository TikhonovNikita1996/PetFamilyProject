using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;

namespace FileService.Infrastructure.Providers;

public interface IFileProvider
{
    Task IsBucketExists(IEnumerable<string> bucketNames, CancellationToken cancellationToken = default);
    Task<UnitResult<CustomErrorsList>> DeleteFiles(
        IEnumerable<FileData> files, CancellationToken cancellationToken = default);
}