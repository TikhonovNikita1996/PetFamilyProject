using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using PetFamily.API.Extensions;
using PetFamily.Application.FileManagement.Delete;
using PetFamily.Application.FileManagement.GetFile;
using PetFamily.Application.FileManagement.Upload;
using PetFamily.Infrastructure.Options;

namespace PetFamily.API.Controllers;

public class FileController : BaseApiController
{
    private readonly IMinioClient _minioClient;
    public const string TEST_BUCKET_NAME = "photos";
    
    public FileController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();

        var request = new UploadFileRequest(
            stream,
            TEST_BUCKET_NAME,
            Guid.NewGuid().ToString());

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{fileName:guid}")]
    public async Task<IActionResult> RemoveFile(
        [FromRoute] Guid fileName,
        [FromServices] DeleteFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteFileRequest(TEST_BUCKET_NAME, fileName.ToString());
        
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok(result.Value);
    }
    
    [HttpGet("{fileName:guid}")]
    public async Task<ActionResult> Get(
        [FromRoute] Guid fileName,
        [FromServices] GetFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFileRequest(TEST_BUCKET_NAME, fileName.ToString());

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    
    
}