using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Volunteers.Domain.Ids;

namespace PetFamily.Volunteers.Domain.Pet;

public class Pet : SoftDeletableEntity<PetId>
{
    private List<Photo> _photos = [];
    // ef core
    public Pet(PetId id) : base(id) {}
    private Pet(PetId petId, PetsName petsName, Age age, SpecieDetails specieDetails,
        GenderType gender, PetsDescription petsDescription,
        Color color, HealthInformation healthInformation, LocationAddress locationAddress,
        double weight, double height, OwnersPhoneNumber ownersPhoneNumber, bool isSterilized,
        bool isVaccinated, HelpStatusType currentStatus, DonationInfoList donateForHelpInfos,
        DateTime petsPageCreationDate) : base(petId)
    {
        PetsName = petsName;
        Age = age;
        SpecieDetails = specieDetails;
        Gender = gender;
        PetsDescription = petsDescription;
        Color = color;
        HealthInformation = healthInformation;
        LocationAddress = locationAddress;
        Weight = weight;
        Height = height;
        OwnersPhoneNumber = ownersPhoneNumber;
        IsSterilized = isSterilized;
        IsVaccinated = isVaccinated;
        CurrentStatus = currentStatus;
        DonateForHelpInfos = donateForHelpInfos;
        PetsPageCreationDate = petsPageCreationDate;
    }
    
    public PetsName PetsName { get; private set; }
    public Age Age { get; private set; }
    public SpecieDetails SpecieDetails { get; private set; }
    public GenderType Gender { get; private set; }
    public PetsDescription PetsDescription { get; private set; }
    public Color Color { get; private set; }
    public HealthInformation HealthInformation { get; private set; } 
    public LocationAddress LocationAddress { get; private set; }
    public double Weight { get; private set; } = default!;
    public double Height { get; private set; } = default!;
    public OwnersPhoneNumber OwnersPhoneNumber { get; private set;}
    public bool IsSterilized { get; private set; }
    public bool IsVaccinated { get; private set; } 
    public HelpStatusType CurrentStatus { get; private set; } 
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public DateTime PetsPageCreationDate { get; private set; }
    
    public IReadOnlyList<Photo> Photos => _photos;
    public PositionNumber PositionNumber { get; private set; }

    public static Result<Pet,CustomError> Create(PetId petId, PetsName petsName, Age Age,
                                            SpecieDetails specieDetails, GenderType gender, 
                                            PetsDescription petsDescription,
                                            Color color, HealthInformation healthInformation, 
                                            LocationAddress locationAddress,
                                            double weight, double height, 
                                            OwnersPhoneNumber ownersPhoneNumber, bool isSterilized, 
                                            bool isVaccinated, HelpStatusType currentStatus, 
                                            DonationInfoList donateForHelpInfos,
                                            DateTime petsPageCreationDate)
    {
        
        var pet = new Pet(petId, petsName, Age,specieDetails, gender, petsDescription,
                          color, healthInformation, locationAddress, weight, 
                          height, ownersPhoneNumber, isSterilized,
                          isVaccinated, currentStatus, donateForHelpInfos, 
                          petsPageCreationDate);
        return pet;
    }
    
    public void UpdateMainInfo(PetsName petsName, 
        SpecieDetails specieDetails,
        GenderType gender, 
        PetsDescription petsDescription,
        LocationAddress locationAddress,
        double weight, double height,
        bool isSterilized, bool isVaccinated,
        OwnersPhoneNumber ownersPhoneNumber,
        Color color)
    {
        PetsName = petsName;
        SpecieDetails = specieDetails;
        Gender = gender;
        PetsDescription = petsDescription;
        LocationAddress = locationAddress;
        Weight = weight;
        Height = height;
        IsSterilized = isSterilized;
        IsVaccinated = isVaccinated;
        OwnersPhoneNumber = ownersPhoneNumber;
        Color = color;
    }
    
    public void UpdatePhotos(List<Photo> photosList)
    {
        _photos = photosList;
    }
        
    public void UpdateStatus(HelpStatusType status)
    {
        CurrentStatus = status;
    }
    
    
    public void SetPositionNumberToPet(PositionNumber positionNumber) =>
        PositionNumber = positionNumber;
    
    public void MoveUp(int currentPetsListCount)
    {
        var newPosition = PositionNumber.Value + 1;
        if (newPosition > currentPetsListCount) return;
        PositionNumber = PositionNumber.Create(PositionNumber.Value + 1).Value;
    }

    public void MoveDown(int currentPetsListCount)
    {
        var newPosition = PositionNumber.Value - 1;
        if (newPosition < 1) return;
        PositionNumber = PositionNumber.Create(newPosition).Value;
    }

    public void SetMainPhoto(Guid fileId)
    {
        foreach (var photo in Photos)
        {
            photo.IsMain = false;
        }
        
        var newMainPhoto = Photos.FirstOrDefault(p => p.FileId == fileId);
        
        if(newMainPhoto != null)
            newMainPhoto.IsMain = true;
    }
}