using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Upload;

public class UploadFileHandler
{
    private readonly IFileProvider _fileProvider;

    public UploadFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, CustomError>> Handle(
        UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var filePath = FilePath.Create(request.FilePath).Value;
        var fileData = new FileData(
            request.FileStream,
            new FileMetaData(request.BucketName, FilePath.Create(request.FilePath).Value));
        
        var result = await _fileProvider.UploadFileAsync(fileData, cancellationToken);

        return result;
    }
}