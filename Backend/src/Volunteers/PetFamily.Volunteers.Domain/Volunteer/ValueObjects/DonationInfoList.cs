namespace PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

public record DonationInfoList
{
    public DonationInfoList() {}

    public IReadOnlyList<DonationInfo> DonationInfos { get; }
    public DonationInfoList(List<DonationInfo> donationInfos) => DonationInfos = donationInfos;
}