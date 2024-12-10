using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileProvider;

public record FileMetaData(string BucketName, FilePath FilePath);