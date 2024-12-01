using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;
        private const int EXPIRY = 60 * 60 * 24;

        public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }
        
        public async Task<Result<string, CustomError>> UploadFile(
            FileData fileData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await CreateBucketIfNotExists(fileData.BucketName, cancellationToken);
            
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(fileData.FileStream)
                    .WithObjectSize(fileData.FileStream.Length)
                    .WithObject(fileData.FilePath);

                var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
                return result.ObjectName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to upload file to minio");
                return CustomError.Failure("file.upload", "Failed to upload file to minio");
            }
        }
        
        public async Task<Result<string, CustomError>> DeleteFile(
            FileMetaData fileMetaData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExist = await IsBucketExist(fileMetaData.BucketName, cancellationToken);
                if (bucketExist == false)
                {
                    return CustomError.Failure("file.delete", $"Bucket {fileMetaData.BucketName} not found");
                }

                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileMetaData.BucketName)
                    .WithObject(fileMetaData.ObjectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

                _logger.LogInformation("Deleted file {objectName} from minio", fileMetaData.ObjectName);

                return fileMetaData.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file from minio");
                return CustomError.Failure("file.delete", "Failed to delete file from minio." +
                                                          " May be file is already deleted.");
            }
        }

        public async Task<Result<string, CustomError>> GetFile(
            FileMetaData fileMetaData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await CreateBucketIfNotExists(fileMetaData.BucketName, cancellationToken);

                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(fileMetaData.BucketName)
                    .WithObject(fileMetaData.ObjectName);
                
                var statObject = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
                if (statObject.Size == 0)
                {
                    return CustomError.Failure("file.get", $"File {fileMetaData.ObjectName} not found");
                }
            
                var presignedObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileMetaData.BucketName)
                    .WithObject(fileMetaData.ObjectName)
                    .WithExpiry(EXPIRY);

                return await _minioClient.PresignedGetObjectAsync(presignedObjectArgs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get file from minio");
                return CustomError.Failure("file.get", "Failed to get file from minio");
            }
        }

        private async Task<bool> IsBucketExist(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);
            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            return bucketExist;
        }
        
        private async Task CreateBucketIfNotExists(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var bucketExist = await IsBucketExist(bucketName, cancellationToken);
            if (bucketExist == false)
            {
                await CreateBucket(bucketName, cancellationToken);
            }
        }
        
        private async Task CreateBucket(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
    }
}