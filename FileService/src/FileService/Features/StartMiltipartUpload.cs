using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class StartMultipartUpload
{
    private record StartMultipartUploadRequest(
        string FileName,
        string ContentType,
        long Size);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(
        StartMultipartUploadRequest request,
        IAmazonS3 s3Client,
        IFileProvider provider,
        CancellationToken cancellationToken)
    {
        try
        {
            var key = Guid.NewGuid().ToString();
            
            await provider.IsBucketExists(["bucket"], cancellationToken);

            var startMultipartRequest = new InitiateMultipartUploadRequest
            {
                BucketName = "bucket",
                Key = key,
                ContentType = request.ContentType,
                Metadata =
                {
                    ["file-name"] = request.FileName
                }
            };

            var response = await s3Client.InitiateMultipartUploadAsync(
                startMultipartRequest,
                cancellationToken);

            return Results.Ok(new
            {
                key,
                uploadId = response.UploadId
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error starting multipart upload: {ex.Message}");
        }
    }
}