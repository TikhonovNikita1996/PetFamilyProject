namespace PetFamily.Application.Dtos;

public record UploadFileDto(
    Stream Stream,
    string FileName);