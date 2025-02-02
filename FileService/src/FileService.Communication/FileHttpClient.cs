using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Contracts;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient) : IFileService
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
}