﻿using CSharpFunctionalExtensions;
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
        private const int MAX_DEGREE_OF_PARALLELISM = 5;
        public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }
        
        public async Task<Result<string, CustomError>> UploadFileAsync(
            FileData fileData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await IfBucketsNotExistCreateBucketAsync([fileData], cancellationToken);
            
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(fileData.FileStream)
                    .WithObjectSize(fileData.FileStream.Length)
                    .WithObject(fileData.FilePath.Path);

                var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
                return result.ObjectName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to upload file to minio");
                return CustomError.Failure("file.upload", "Failed to upload file to minio");
            }
        }
        
        public async Task<Result<IReadOnlyList<FilePath>, CustomError>> UploadFilesAsync(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
            var filesList = filesData.ToList();

            try
            {
                await IfBucketsNotExistCreateBucketAsync(filesList, cancellationToken);

                var tasks = filesList.Select(async file =>
                    await PutObject(file, semaphoreSlim, cancellationToken));

                var pathsResult = await Task.WhenAll(tasks);

                if (pathsResult.Any(p => p.IsFailure))
                    return pathsResult.First().Error;

                var results = pathsResult.Select(p => p.Value).ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload files in minio, files amount: {amount}", filesList.Count);

                return CustomError.Failure("file.upload", "Fail to upload files in minio");
            }
        }
        
        public async Task<Result<string, CustomError>> DeleteFileAsync(
            FileMetaData fileMetaData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExist = await IsBucketExistAsync(fileMetaData.BucketName, cancellationToken);
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
        
        public async Task<Result<string, CustomError>> GetFileAsync(
            FileMetaData fileMetaData,
            CancellationToken cancellationToken = default)
        {
            try
            {
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

        private async Task IfBucketsNotExistCreateBucketAsync(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken)
        {
            HashSet<string> bucketNames = [..filesData.Select(file => file.BucketName)];

            foreach (var bucketName in bucketNames)
            {
                var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExist = await _minioClient
                    .BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }
            }
        }
        
        private async Task<Result<FilePath, CustomError>> PutObject(
            FileData fileData,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.FileStream)
                .WithObjectSize(fileData.FileStream.Length)
                .WithObject(fileData.FilePath.Path);

            try
            {
                await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                return fileData.FilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.FilePath.Path,
                    fileData.BucketName);

                return CustomError.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        
        private async Task<bool> IsBucketExistAsync(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);
            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            return bucketExist;
        }
    }
}