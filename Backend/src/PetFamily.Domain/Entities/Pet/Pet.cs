using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Entities.Pet;

public class Pet : BaseEntity<PetId>
{
    // ef core
    public Pet(PetId id) : base(id)
    {
        
    }
    
    public Pet(PetId petId, string petsName, string species, GenderType gender, string description,
        string breed, string color, string healthInformation, LocationAddress locationAddress,
        double weight, double hight, string ownersPhoneNumber, bool isSterilized, DateTime dateOfBirth,
        bool isVaccinated, HelpStatusType currentStatus, DonationInfoList donateForHelpInfos,
        DateTime petsPageCreationDate) : base(petId)
    {
        PetsName = petsName;
        Species = species;
        Gender = gender;
        Description = description;
        Breed = breed;
        Color = color;
        HealthInformation = healthInformation;
        LocationAddress = locationAddress;
        Weight = weight;
        Hight = hight;
        OwnersPhoneNumber = ownersPhoneNumber;
        IsSterilized = isSterilized;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        CurrentStatus = currentStatus;
        DonateForHelpInfos = donateForHelpInfos;
        PetsPageCreationDate = petsPageCreationDate;
    }
    
    public string PetsName { get; private set; }
    public string Species { get; private set; }
    public GenderType Gender { get; private set; }
    public string Description { get; private set; }
    public string Breed { get; private set; }
    public string Color { get; private set; }
    public string HealthInformation { get; private set; } 
    public LocationAddress LocationAddress { get; private set; }
    public double Weight { get; private set; } = default!;
    public double Hight { get; private set; } = default!;
    public string OwnersPhoneNumber { get; private set;}
    public bool IsSterilized { get; private set; }
    public DateTime DateOfBirth { get; private set; } 
    public bool IsVaccinated { get; private set; } 
    public HelpStatusType CurrentStatus { get; private set; } 
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public DateTime PetsPageCreationDate { get; private set; } 
    public Photos Photos { get; private set; }

    public static CustomResult<Pet> Create(PetId petId, string petsName, 
                                            string species, GenderType gender, string description,
                                            string breed, string color, string healthInformation, 
                                            LocationAddress locationAddress,
                                            double weight, double hight, 
                                            string ownersPhoneNumber, bool isSterilized, 
                                            DateTime dateOfBirth,
                                            bool isVaccinated, HelpStatusType currentStatus, 
                                            DonationInfoList donateForHelpInfos,
                                            DateTime petsPageCreationDate)
    {
        var pet = new Pet(petId, petsName, species, gender, description, breed,
                          color, healthInformation, locationAddress, weight, 
                          hight, ownersPhoneNumber, isSterilized, dateOfBirth, 
                          isVaccinated, currentStatus, donateForHelpInfos, 
                          petsPageCreationDate);

        return pet;
    }
}