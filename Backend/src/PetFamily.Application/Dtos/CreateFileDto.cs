namespace PetFamily.Application.Dtos;

public record CreateFileDto(
    Stream Stream,
    string FileName);