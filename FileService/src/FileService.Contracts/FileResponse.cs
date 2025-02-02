namespace FileService.Contracts;

public record FileResponse(Guid FileId, string PresignedUrl);