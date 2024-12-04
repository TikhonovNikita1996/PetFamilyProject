using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Delete;

public class DeleteFileHandler
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, CustomError>> Handle(
        DeleteFileRequest request, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(request.BucketName, request.ObjectName);
        var result = await _fileProvider.DeleteFileAsync(fileMetaData, cancellationToken);

        return result;
    }
}