namespace Pet.Family.SharedKernel.ValueObjects.Volunteer;

public record DonationInfoList
{
    public DonationInfoList() {}

    public IReadOnlyList<DonationInfo> DonationInfos { get; }
    public DonationInfoList(List<DonationInfo> donationInfos) => DonationInfos = donationInfos;
}