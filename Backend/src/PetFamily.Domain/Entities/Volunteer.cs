using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities;

public class Volunteer
{
    // ef core
    public Volunteer()
    {
        
    }
    public Guid Id { get; private set; }
    public string FIO { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public int WorkingExperience { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    /*public DonationInfo DonateForHelpInfos { get; private set; }*/
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public List<Pet> CurrentPets { get; private set; } = [];
    public int PetsWhoFoundHome => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.FoundHome).Count();
    public int PetsSearchingForHome => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.SerachingForHome).Count();
    public int PetsOnTreatment => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.OnTreatment).Count();
    
}