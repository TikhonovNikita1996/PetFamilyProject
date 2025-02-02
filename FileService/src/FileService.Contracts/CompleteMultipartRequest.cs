namespace FileService.Contracts;

public record CompleteMultipartRequest(string UploadId, List<PartETagInfo> Parts);