using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    public IEnumerable<ResponseError> Errors { get; } = [];
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Success(object? result = null) =>
        new(result, []);
    public static Envelope Failure(IEnumerable<ResponseError>? errors) =>
        new(null, errors);
}