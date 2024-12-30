using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Pet;

public record Photo
{
    // ef core
    public Photo()
    {
        
    }
    private Photo(string filepath, bool isMain)
    {
        FilePath = filepath;
        IsMain = isMain;
    }
    public string FilePath { get; set; }
    public bool IsMain { get; set; }
    public static Result<Photo, CustomError> Create(string path,
        bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid(nameof(Path));

        var newPhoto = new Photo(path, isMain);

        return newPhoto;
    }
}