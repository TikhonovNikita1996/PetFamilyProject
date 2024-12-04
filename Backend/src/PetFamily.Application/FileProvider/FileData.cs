using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileProvider;

public record FileData(
    Stream FileStream,
    string BucketName,
    FilePath FilePath);