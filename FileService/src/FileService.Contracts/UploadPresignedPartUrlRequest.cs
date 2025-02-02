namespace FileService.Contracts;

public record UploadPresignedPartUrlRequest(string UploadId, int PartNumber);