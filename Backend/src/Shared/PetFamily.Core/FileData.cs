namespace PetFamily.Core;

public record FileData(
    Stream FileStream,
    FileMetaData FileMetaData);