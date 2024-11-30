namespace PetFamily.Application.FileManagement.Delete;

public record DeleteFileRequest(
    string BucketName,
    string ObjectName);