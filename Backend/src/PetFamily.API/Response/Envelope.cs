using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record Envelope
{
    public object? Result { get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, CustomError? error)
    {
        Result = result;
        ErrorCode = error?.Code;
        ErrorMessage = error?.Message;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Success(object? result = null) =>
        new(result, null);
    public static Envelope Failure(CustomError error) =>
        new(null, error);
}