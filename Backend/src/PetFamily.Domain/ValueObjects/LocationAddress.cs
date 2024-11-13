using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

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
    
    public static CustomResult<LocationAddress> Create(string region,
        string city,
        string street,
        string houseNumber,
        string? floor,
        string? apartment)
    {
        if (string.IsNullOrEmpty(region))
            return "Region can not be empty";
        if (string.IsNullOrEmpty(city))
            return "City can not be empty";
        if (string.IsNullOrEmpty(street))
            return "Street can not be empty";
        if (string.IsNullOrEmpty(houseNumber))
            return "House number can not be empty";

        var newLocationAddressAddress = new LocationAddress(region, city, street, houseNumber,
            floor, apartment);

        return newLocationAddressAddress;
    }
}