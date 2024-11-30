namespace PetFamily.Application.FileManagement.GetFile;

public record GetFileRequest(
    string BucketName,
    string ObjectName);