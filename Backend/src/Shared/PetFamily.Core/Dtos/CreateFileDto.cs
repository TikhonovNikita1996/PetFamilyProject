namespace PetFamily.Core.Dtos;

public record CreateFileDto(
    string FileName,
    string ContentType,
    int FileSize);