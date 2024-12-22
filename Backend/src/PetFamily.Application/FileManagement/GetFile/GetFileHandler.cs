
using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.GetFile;

public class GetFileHandler
{
    private readonly IFileService _fileService;

    public GetFileHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<Result<string, CustomError>> Handle(
        GetFileRequest request, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(request.BucketName, FilePath.Create(request.ObjectName).Value);
        var result = await _fileService.GetFileAsync(fileMetaData, cancellationToken);

        return result;
    }
}