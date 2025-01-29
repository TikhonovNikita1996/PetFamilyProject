using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;

namespace FileService.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IAmazonS3 _s3Client;

    public MinioProvider(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task IsBucketExists(
        IEnumerable<string> bucketNames, CancellationToken cancellationToken = default)
    {
        HashSet<string> buckets = [..bucketNames];

        var response = await _s3Client.ListBucketsAsync(cancellationToken);

        foreach (var bucketName in buckets)
        {
            var isExists = response.Buckets
                .Exists(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

            if (!isExists)
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName
                };

                await _s3Client.PutBucketAsync(request, cancellationToken);
            }
        }
    }

    public Task<UnitResult<CustomErrorsList>> DeleteFiles(IEnumerable<FileData> files,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}