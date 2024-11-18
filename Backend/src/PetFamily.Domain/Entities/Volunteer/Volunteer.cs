using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteer;

public class Volunteer : BaseEntity<VolunteerId>
{
    private readonly List<Pet.Pet> _pets = [];
    
    // ef core
    public Volunteer(VolunteerId id) : base(id)
    {
        
    }
    public Volunteer(VolunteerId id, FullName fullname, int age, Email email,
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
    public Email Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public int WorkingExperience { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public IReadOnlyList<Pet.Pet> CurrentPets => _pets;
    public int PetsWhoFoundHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.FoundHome);
    public int PetsSearchingForHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.SerachingForHome);
    public int PetsOnTreatment => _pets.Count(p => p.CurrentStatus == HelpStatusType.OnTreatment);
    
    public static Result<Volunteer, CustomError> Create(VolunteerId id,
                                                FullName fullname, int age, Email email,
                                                GenderType gender, int workingExperience, string description,
                                                string phoneNumber, DonationInfoList donationInfoList,
                                                SocialMediaDetails socialMediaDetails)
    {
        if (age > 0)
            return Errors.General.DigitValueIsInvalid("Age");
        if (workingExperience < 0)
            return Errors.General.ValueIsInvalid("Working Experience");
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description");
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return  Errors.General.ValueIsInvalid("PhoneNumber");
        
        var volunteer = new Volunteer(id, fullname, age, email, gender, workingExperience, description,
            phoneNumber, donationInfoList, socialMediaDetails);

        return volunteer;
    }
    
}