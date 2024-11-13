using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Entities.Volunteer;

public class Volunteer : BaseEntity<VolunteerId>
{
    
    // ef core
    public Volunteer(VolunteerId id) : base(id)
    {
        
    }
    public Volunteer(VolunteerId id, FullName fullname, int age, string email,
        GenderType gender, int workingExperience, string description,
        string phoneNumber, DonationInfoList donationInfoList, SocialMediaDetails socialMediaDetails) : base(id)
    {
        Fullname = fullname;
        Age = age;
        Email = email;
        Gender = gender;
        WorkingExperience = workingExperience;
        Description = description;
        PhoneNumber = phoneNumber;
        DonateForHelpInfos = donationInfoList;
        SocialMediaDetails = socialMediaDetails;
    }
    public FullName Fullname { get; private set; }
    public int Age { get; private set; } = default!;
    public string Email { get; private set; }
    public GenderType Gender { get; private set; }
    public int WorkingExperience { get; private set; } = default!;
    public string Description { get; private set; } 
    public string PhoneNumber { get; private set; } 
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public IReadOnlyList<Pet.Pet> CurrentPets { get; private set; } = [];
    public int PetsWhoFoundHome => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.FoundHome).Count();
    public int PetsSearchingForHome => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.SerachingForHome).Count();
    public int PetsOnTreatment => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.OnTreatment).Count();


    public static CustomResult<Volunteer> Create(VolunteerId id,
                                                FullName fullname, int age, string email,
                                                GenderType gender, int workingExperience, string description,
                                                string phoneNumber, DonationInfoList donationInfoList,
                                                SocialMediaDetails socialMediaDetails)
    {
        var volunteer = new Volunteer(id, fullname, age, email, gender, workingExperience, description,
            phoneNumber, donationInfoList, socialMediaDetails);

        return volunteer;
    }
}