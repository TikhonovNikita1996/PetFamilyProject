using FileService.Contracts;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;

namespace FileService.Features;

public static class DeleteFiles
{
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/delete", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DeleteFilesRequest request,
        IFilesRepository filesRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var files = await filesRepository.Get(request.FileIds, cancellationToken);
        
        var result = await provider.DeleteFiles(files, cancellationToken);
        
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        await filesRepository.DeleteMany(request.FileIds, cancellationToken);
        
        return Results.Ok();
    }
}