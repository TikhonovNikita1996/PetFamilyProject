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
    public static CustomResult<DonationInfo> Create(string name,
        string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name can not be empty";
        if (string.IsNullOrWhiteSpace(description))
            return "Description can not be empty";
        
        var donationInfo = new DonationInfo(name, description);

        return donationInfo;
    }
}