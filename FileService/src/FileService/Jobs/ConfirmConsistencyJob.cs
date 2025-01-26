using Amazon.S3;
using FileService.Infrastructure;
using FileService.Infrastructure.Repositories;
using Hangfire;

namespace FileService.Jobs;

public class ConfirmConsistencyJob(
    IFilesRepository filesRepository,
    IAmazonS3 s3Client,
    ILogger<ConfirmConsistencyJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(Guid fileId, string key)
    {
        logger.LogInformation("Start ConfirmConsistencyJob with {fileId} and {key}", fileId, key);

        await Task.Delay(3000);

        logger.LogInformation("End ConfirmConsistencyJob");
    }
}