
using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.GetFile;

public class GetFileHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, CustomError>> Handle(
        GetFileRequest request, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(request.BucketName, request.ObjectName);
        var result = await _fileProvider.GetFileAsync(fileMetaData, cancellationToken);

        return result;
    }
}