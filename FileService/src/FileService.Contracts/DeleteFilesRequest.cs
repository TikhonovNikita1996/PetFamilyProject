namespace FileService.Contracts;

public record DeleteFilesRequest(IEnumerable<Guid> FileIds);