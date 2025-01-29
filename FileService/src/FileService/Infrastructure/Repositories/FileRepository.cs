using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;
using MongoDB.Driver;

namespace FileService.Infrastructure.Repositories;

public class FilesRepository : IFilesRepository
{
    private readonly FileMongoDbContext _mongoDbContext;

    public FilesRepository(
        FileMongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public async Task<Result<Guid, CustomError>> Add(FileData fileData, CancellationToken cancellationToken)
    {
        await _mongoDbContext.Files.InsertOneAsync(fileData, cancellationToken: cancellationToken);

        return fileData.Id;
    }

    public async Task<IReadOnlyCollection<FileData>> Get(IEnumerable<Guid> fileIds, CancellationToken cancellationToken)
        => await _mongoDbContext.Files.Find(f => fileIds.Contains(f.Id)).ToListAsync(cancellationToken);

    public async Task<UnitResult<CustomError>> DeleteMany(IEnumerable<Guid> fileIds,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _mongoDbContext.Files.DeleteManyAsync(f => fileIds.Contains(f.Id),
            cancellationToken: cancellationToken);

        if (deleteResult.DeletedCount == 0)
            return Errors.Files.FailRemove();

        return Result.Success<CustomError>();
    }
}