using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Entities.Others;

public record DonationInfoList
{
    public List<DonationInfo> DonationInfos { get; }
}