using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }

    public CustomErrorsList? Errors { get; }

    public DateTime TimeGenerated { get; }

    private Envelope(object? result, CustomErrorsList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Failure(CustomErrorsList errors) =>
        new(null, errors);
}