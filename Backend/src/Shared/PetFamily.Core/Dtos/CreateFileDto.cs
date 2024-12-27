namespace PetFamily.Core.Dtos;

public record CreateFileDto(
    Stream Stream,
    string FileName);