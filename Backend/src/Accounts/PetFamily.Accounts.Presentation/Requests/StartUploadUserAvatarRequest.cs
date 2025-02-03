namespace PetFamily.Accounts.Presentation.Requests;

public record StartUploadUserAvatarRequest(string FileName, string ContentType, int Size);