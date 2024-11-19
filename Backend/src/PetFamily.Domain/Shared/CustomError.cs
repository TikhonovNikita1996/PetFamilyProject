namespace PetFamily.Domain.Shared;

public record CustomError
{
    public const string SEPORATOR = "||";
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

    public string Serialize()
    {
        return string.Join(SEPORATOR, Code, Message, Type);
    }

    public static CustomError DeSerialize(string serialized)
    {
        var parts = serialized.Split(SEPORATOR);
        if (parts.Length < 3)
            throw new ArgumentException("Invalid serialized format");
        
        if(Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new ArgumentException("Invalid serialized format");
        
        return new CustomError(parts[0], parts[1], type);
    }
}