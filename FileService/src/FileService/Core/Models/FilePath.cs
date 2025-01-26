using CSharpFunctionalExtensions;

namespace FileService.Core.Models;

public record FilePath
{
    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

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