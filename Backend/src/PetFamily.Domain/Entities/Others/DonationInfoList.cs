using PetFamily.Domain.Entities.Volunteer.ValueObjects;

namespace PetFamily.Domain.Entities.Others;

public record DonationInfoList
{
    public DonationInfoList()
    {
        
    }
    public IReadOnlyList<DonationInfo> DonationInfos { get; }
    public DonationInfoList(List<DonationInfo> donationInfos) => DonationInfos = donationInfos;
}