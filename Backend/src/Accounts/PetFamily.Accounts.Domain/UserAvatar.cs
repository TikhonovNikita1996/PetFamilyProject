using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Accounts.Domain;

public class UserAvatar
{
    private static readonly string[] PermittedFileTypes = ["image/jpeg","image/jpg","image/png"];
    private static readonly string[] PermittedFileExtensions =
        ["png", "jpeg", "jpg"];
    
    public const int MAX_FILE_SIZE = 5120;
    
    // ef core
    public UserAvatar(){}
    public UserAvatar(Guid fileId)
    {
        FileId = fileId;
    }
    public Guid FileId { get; set; }
    public bool IsMain { get; set; }
    public static UnitResult<CustomError> Validate(
        string fileName,
        string contentType,
        long size)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Errors.General.ValueIsInvalid(fileName);

        var fileExtension = fileName.Split(".").Last();

        if (!PermittedFileExtensions.Contains(fileExtension))
            return Errors.Files.InvalidExtension();
        
        if (!PermittedFileTypes.Contains(contentType))
            return Errors.General.ValueIsInvalid(contentType);
        
        if (size > MAX_FILE_SIZE)
            return Errors.Files.InvalidSize();
        
        return Result.Success<CustomError>();
    }
}