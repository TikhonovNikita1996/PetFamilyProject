namespace FileService.Contracts;

public record StartMultipartUploadRequest(
    string FileName,
    string ContentType,
    int Size);