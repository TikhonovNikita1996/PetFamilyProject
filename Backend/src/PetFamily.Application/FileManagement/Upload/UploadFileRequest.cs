namespace PetFamily.Application.FileManagement.Upload;

public record UploadFileRequest(
    Stream FileStream,
    string BucketName,
    string FilePath);