namespace FileService.Contracts;

public record StartMultipartUploadRequest(
    string FileName,
    string ContentType,
    long Size);