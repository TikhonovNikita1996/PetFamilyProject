using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Entities.Volunteer;

public class Volunteer : BaseEntity<VolunteerId>
{
    private readonly List<Pet.Pet> _pets = [];
    
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
    public FullName Fullname { get; private set; } = default!;
    public int Age { get; set; }
    public string Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public int WorkingExperience { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public IReadOnlyList<Pet.Pet> CurrentPets => _pets;
    public int PetsWhoFoundHome => _pets.Where(p => p.CurrentStatus == HelpStatusType.FoundHome).Count();
    public int PetsSearchingForHome => _pets.Where(p => p.CurrentStatus == HelpStatusType.SerachingForHome).Count();
    public int PetsOnTreatment => _pets.Where(p => p.CurrentStatus == HelpStatusType.OnTreatment).Count();
    
    public static CustomResult<Volunteer> Create(VolunteerId id,
                                                FullName fullname, int age, string email,
                                                GenderType gender, int workingExperience, string description,
                                                string phoneNumber, DonationInfoList donationInfoList,
                                                SocialMediaDetails socialMediaDetails)
    {
        if (age > 0)
            return "Age must be greater than zero";
        if (string.IsNullOrWhiteSpace(email))
            return "Email name can not be empty";
        if (workingExperience >= 0)
            return "Experience cannot be zero";
        if (string.IsNullOrWhiteSpace(description))
            return "Description name can not be empty";
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return "Phone number name can not be empty";
        
        var volunteer = new Volunteer(id, fullname, age, email, gender, workingExperience, description,
            phoneNumber, donationInfoList, socialMediaDetails);

        return volunteer;
    }
    
}