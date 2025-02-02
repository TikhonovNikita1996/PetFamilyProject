namespace FileService.Contracts;

public record GetFilePresignedUrlRequest(IEnumerable<Guid> FileIds);