using FileService.Endpoints;
using FileService.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FileService.Features;

public static class TestHangFire
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("hangfire/test", Handler);
        }
    }

    private static IResult Handler(
        CancellationToken cancellationToken)
    {
        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(j => j.Execute(Guid.NewGuid(), "key"), TimeSpan.FromSeconds(5));

        return Results.Ok(jobId);
    }
}