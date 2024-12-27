using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

public record LocationAddress
{
    private LocationAddress(string region,
        string city,
        string street,
        string houseNumber,
        string? floor,
        string? apartment)
    {
        Region = region;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        Floor = floor;
        Apartment = apartment;
    }
    public string Region { get; } = default!;
    public string City { get; } = default!;
    public string Street { get; } = default!;
    public string HouseNumber { get; } = default!;
    public string? Floor { get; }
    public string? Apartment { get; }
    
    public static Result<LocationAddress, CustomError> Create(string region,
        string city,
        string street,
        string houseNumber,
        string? floor,
        string? apartment)
    {
        if (string.IsNullOrWhiteSpace(region))
            return Errors.General.ValueIsInvalid(region);
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsInvalid(city);
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsInvalid(street);
        if (string.IsNullOrWhiteSpace(houseNumber))
            return Errors.General.ValueIsInvalid(houseNumber);

        var newLocationAddressAddress = new LocationAddress(region, city, street, houseNumber,
            floor, apartment);

        return newLocationAddressAddress;
    }
}