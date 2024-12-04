using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;

public record PetPhoto
{
    // ef core
    public PetPhoto()
    {
        
    }
    private PetPhoto(string filepath, bool isMain)
    {
        FilePath = filepath;
        IsMain = isMain;
    }
    public string FilePath { get; }
    public bool IsMain { get; }
    public static Result<PetPhoto, CustomError> Create(string path,
        bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid(nameof(Path));

        var newPhoto = new PetPhoto(path, isMain);

        return newPhoto;
    }
}