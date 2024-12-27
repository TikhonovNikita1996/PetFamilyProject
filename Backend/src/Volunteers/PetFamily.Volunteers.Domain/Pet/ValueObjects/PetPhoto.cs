using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pet.ValueObjects;

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
    public string FilePath { get; set; }
    public bool IsMain { get; set; }
    public static Result<PetPhoto, CustomError> Create(string path,
        bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid(nameof(Path));

        var newPhoto = new PetPhoto(path, isMain);

        return newPhoto;
    }
}