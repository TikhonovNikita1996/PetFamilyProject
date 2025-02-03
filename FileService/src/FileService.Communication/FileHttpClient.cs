using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Contracts;

namespace FileService.Communication;

internal class FileHttpClient(HttpClient httpClient) : IFileService
{
    public async Task<Result<IReadOnlyList<FileResponse>>> GetPresignedUrls(GetFilePresignedUrlRequest request
        ,CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("files/presigned-urls", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return Result.Failure<IReadOnlyList<FileResponse>>("Failed to get presigned urls");
        }
        
        var fileResponse = await response.Content.ReadFromJsonAsync<IReadOnlyList<FileResponse>>(cancellationToken);
        
        return fileResponse?.ToList() ?? [];
    }

    public async Task<Result<FileResponse, string>> StartMultipartUpload(StartMultipartUploadRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("files/start-multipart", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return "Failed to start multipart upload";
        }

        var fileResponse = await response.Content.ReadFromJsonAsync<FileResponse>(cancellationToken);

        return fileResponse!;
    }

    public async Task<Result<FileResponse, string>> CompleteMultipartUpload(CompleteMultipartRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            $"files/{request.UploadId}/complete-multipart",
            request,
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return "Failed to complete multipart upload";
        }

        var fileResponse = await response.Content.ReadFromJsonAsync<FileResponse>(cancellationToken);

        return fileResponse!;
    }
}