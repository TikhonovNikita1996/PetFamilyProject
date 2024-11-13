﻿using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteer;

public class Volunteer : BaseEntity<VolunteerId>
{
    private readonly List<Pet.Pet> _pets = [];
    
    // ef core
    public Volunteer(VolunteerId id) : base(id)
    {
        
    }
    public string FIO { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public int WorkingExperience { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public DonationInfo DonateForHelpInfos { get; private set; }
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public IReadOnlyList<Pet.Pet> CurrentPets => _pets;
    public int PetsWhoFoundHome => _pets.Where(p => p.CurrentStatus == HelpStatusType.FoundHome).Count();
    public int PetsSearchingForHome => _pets.Where(p => p.CurrentStatus == HelpStatusType.SerachingForHome).Count();
    public int PetsOnTreatment => _pets.Where(p => p.CurrentStatus == HelpStatusType.OnTreatment).Count();
    
}