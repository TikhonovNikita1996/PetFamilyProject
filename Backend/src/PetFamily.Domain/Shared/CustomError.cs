namespace PetFamily.Domain.Shared;

public record CustomError
{
    public string Code { get; }
    public string Message { get;}
    public ErrorType Type { get; }

    private CustomError(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }
    
    public static CustomError Validation(string code, string message) =>
        new CustomError(code, message, ErrorType.Validation);
    public static CustomError NotFound(string code, string message) =>
        new CustomError(code, message, ErrorType.NotFound);
    public static CustomError Conflict(string code, string message) =>
        new CustomError(code, message, ErrorType.Conflict);
    public static CustomError Failure(string code, string message) =>
        new CustomError(code, message, ErrorType.Failure);
    
}