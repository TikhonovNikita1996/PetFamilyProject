using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Repositories;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
    private record PartETagInfo(int PartNumber, string ETag);

    private record CompleteMultipartRequest(string UploadId, List<PartETagInfo> Parts);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/complete-multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(
        string key,
        CompleteMultipartRequest request,
        IFilesRepository filesRepository,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var fileId = Guid.NewGuid();

            var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(j => j.Execute(fileId, key), TimeSpan.FromSeconds(5));

            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = "bucket",
                Key = key,
                UploadId = request.UploadId,
                PartETags = request.Parts.Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            var response = await s3Client.CompleteMultipartUploadAsync(
                completeRequest,
                cancellationToken);

            var metaDataRequest = new GetObjectMetadataRequest
            {
                BucketName = "bucket",
                Key = key
            };

            var metaData = await s3Client.GetObjectMetadataAsync(metaDataRequest, cancellationToken);

            var fileData = new FileData
            {
                Id = fileId,
                StoragePath = key,
                FileSize = metaData.Headers.ContentLength,
                ContentType = metaData.Headers.ContentType,
                UploadDate = DateTime.UtcNow
            };

            await filesRepository.Add(fileData, cancellationToken);

            BackgroundJob.Delete(jobId);

            return Results.Ok(new
            {
                Id = key,

                location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error complete multipart upload: {ex.Message}");
        }
    }
}