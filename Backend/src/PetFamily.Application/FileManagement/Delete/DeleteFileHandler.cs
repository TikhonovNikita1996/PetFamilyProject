using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Delete;

public class DeleteFileHandler
{
    private readonly IFileService _fileService;

    public DeleteFileHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<UnitResult<CustomError>> Handle(
        DeleteFileRequest request, 
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(request.BucketName, FilePath.Create(request.ObjectName).Value);
        var result = await _fileService.DeleteFileAsync(fileMetaData, cancellationToken);

        return result;
    }
}