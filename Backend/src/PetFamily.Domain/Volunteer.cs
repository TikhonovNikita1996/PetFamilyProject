using PetFamily.Domain.Shared;

namespace PetFamily.Domain;

public class Volunteer
{
    public Guid Id { get; private set; }
    public string FIO { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public int WorkingExperience { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public List<DetailForAssistance> DetailsForAssistance { get; private set; } = [];
    public List<SocialMediaDetail> SocialMediaDetails { get; private set; } = [];
    public List<Pet> CurrentPets { get; private set; } = [];
    public List<Pet> PetsWhoFoundHome => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.FoundHome).ToList();
    public List<Pet> PetsSearchingForHome => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.SerachingForHome).ToList();
    public List<Pet> PetsOnTreatment => CurrentPets.Where(p => p.CurrentStatus == HelpStatusType.OnTreatment).ToList();
    
    

}