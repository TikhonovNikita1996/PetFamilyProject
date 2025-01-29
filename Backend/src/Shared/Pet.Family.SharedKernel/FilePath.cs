using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel;

public record FilePath
{
    private FilePath(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<FilePath, CustomError> Create(Guid path, string extension)
    {
        var fullPath = path + "." + extension;

        return new FilePath(fullPath);
    }
    
    public static Result<FilePath, CustomError> Create(string fullPath)
    {
        return new FilePath(fullPath);
    }
}