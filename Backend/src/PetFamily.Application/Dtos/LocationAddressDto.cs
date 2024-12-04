namespace PetFamily.Application.Dtos;

public record LocationAddressDto (
    string Region,
    string City,
    string Street,
    string HouseNumber,
    string? Floor,
    string? Apartment);