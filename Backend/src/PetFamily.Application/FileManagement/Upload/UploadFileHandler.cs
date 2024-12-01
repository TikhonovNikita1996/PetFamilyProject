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
        var fileData = new FileData(
            request.FileStream,
            request.BucketName,
            request.FilePath);
        
        var result = await _fileProvider.UploadFile(fileData, cancellationToken);

        return result;
    }
}