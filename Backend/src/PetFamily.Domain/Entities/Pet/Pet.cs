using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;
using Description = PetFamily.Domain.Entities.Pet.ValueObjects.Description;

namespace PetFamily.Domain.Entities.Pet;

public class Pet : BaseEntity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    // ef core
    public Pet(PetId id) : base(id) {}
    public Pet(PetId petId, PetsName petsName, SpecieDetails specieDetails, GenderType gender, Description description,
        Color color, HealthInformation healthInformation, LocationAddress locationAddress,
        double weight, double height, OwnersPhoneNumber ownersPhoneNumber, bool isSterilized, DateTime dateOfBirth,
        bool isVaccinated, HelpStatusType currentStatus, DonationInfoList donateForHelpInfos,
        DateTime petsPageCreationDate, PhotosList photosList) : base(petId)
    {
        PetsName = petsName;
        SpecieDetails = specieDetails;
        Gender = gender;
        Description = description;
        Color = color;
        HealthInformation = healthInformation;
        LocationAddress = locationAddress;
        Weight = weight;
        Height = height;
        OwnersPhoneNumber = ownersPhoneNumber;
        IsSterilized = isSterilized;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        CurrentStatus = currentStatus;
        DonateForHelpInfos = donateForHelpInfos;
        PetsPageCreationDate = petsPageCreationDate;
        PhotosList = photosList;
    }
    
    public PetsName PetsName { get; private set; }
    public SpecieDetails SpecieDetails { get; private set; }
    public GenderType Gender { get; private set; }
    public Description Description { get; private set; }
    public Color Color { get; private set; }
    public HealthInformation HealthInformation { get; private set; } 
    public LocationAddress LocationAddress { get; private set; }
    public double Weight { get; private set; } = default!;
    public double Height { get; private set; } = default!;
    public OwnersPhoneNumber OwnersPhoneNumber { get; private set;}
    public bool IsSterilized { get; private set; }
    public DateTime DateOfBirth { get; private set; } 
    public bool IsVaccinated { get; private set; } 
    public HelpStatusType CurrentStatus { get; private set; } 
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public DateTime PetsPageCreationDate { get; private set; } 
    public PhotosList PhotosList { get; private set; }

    public static Result<Pet,CustomError> Create(PetId petId, PetsName petsName, 
                                            SpecieDetails specieDetails, GenderType gender, 
                                            Description description,
                                            Color color, HealthInformation healthInformation, 
                                            LocationAddress locationAddress,
                                            double weight, double height, 
                                            OwnersPhoneNumber ownersPhoneNumber, bool isSterilized, 
                                            DateTime dateOfBirth,
                                            bool isVaccinated, HelpStatusType currentStatus, 
                                            DonationInfoList donateForHelpInfos,
                                            DateTime petsPageCreationDate, PhotosList? photos)
    {
        var pet = new Pet(petId, petsName, specieDetails, gender, description,
                          color, healthInformation, locationAddress, weight, 
                          height, ownersPhoneNumber, isSterilized, dateOfBirth, 
                          isVaccinated, currentStatus, donateForHelpInfos, 
                          petsPageCreationDate, photos);
        return pet;
    }

    public void Delete()
    {
        if (!_isDeleted)
            _isDeleted = true;
    }
    
    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }

    public void UpdatePhotos(PhotosList photosList)
    {
        PhotosList = photosList;
    }

}