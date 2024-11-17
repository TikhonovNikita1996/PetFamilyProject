using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;
public record DonationInfo
{
    private DonationInfo(string name,
        string description)
    {
        Name = name;
        Description = description;
    }
    public string Name { get; } = default!;
    public string Description { get; } = default!;
    public static Result<DonationInfo, CustomError> Create(string name,
        string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(name);
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid(description);
        
        var donationInfo = new DonationInfo(name, description);

        return donationInfo;
    }
}