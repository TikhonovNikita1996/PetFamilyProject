using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;
using Description = PetFamily.Domain.Entities.Volunteer.ValueObjects.Description;

namespace PetFamily.Domain.Entities.Volunteer;

public class Volunteer : BaseEntity<VolunteerId>, ISoftDeletable
{
    private readonly List<Pet.Pet> _pets = [];
    private bool _isDeleted = false;
    
    // ef core
    public Volunteer(VolunteerId id) : base(id) {}

    public Volunteer(VolunteerId id, FullName fullname, Email email,
        GenderType gender, WorkingExperience workingExperience,
        Description description, PhoneNumber phoneNumber,
        DonationInfoList donationInfoList, SocialMediaDetails socialMediaDetails) : base(id)
    {
        Fullname = fullname;
        Email = email;
        Gender = gender;
        WorkingExperience = workingExperience;
        Description = description;
        PhoneNumber = phoneNumber;
        DonateForHelpInfos = donationInfoList;
        SocialMediaDetails = socialMediaDetails;
    }
    public FullName Fullname { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public WorkingExperience WorkingExperience { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public IReadOnlyList<Pet.Pet> CurrentPets => _pets;
    public int PetsWhoFoundHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.FoundHome);
    public int PetsSearchingForHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.SearchingForHome);
    public int PetsOnTreatment => _pets.Count(p => p.CurrentStatus == HelpStatusType.OnTreatment);
    
    public static Result<Volunteer, CustomError> Create(VolunteerId id,
                                                FullName fullname, Email email,
                                                GenderType gender, WorkingExperience workingExperience, Description description,
                                                PhoneNumber phoneNumber, DonationInfoList donationInfoList,
                                                SocialMediaDetails socialMediaDetails)
    {
        
        var volunteer = new Volunteer(id, fullname, email, gender, workingExperience, description,
            phoneNumber, donationInfoList, socialMediaDetails);

        return volunteer;
    }
    
    public void UpdateMainInfo(FullName fullName, 
        Description description, 
        PhoneNumber phoneNumber, 
        WorkingExperience workingExperience)
    {
        Fullname = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        WorkingExperience = workingExperience;
    }
    
    public void UpdateDonationInfo(DonationInfoList NewDonateForHelpInfos)
    {
        DonateForHelpInfos = NewDonateForHelpInfos;
    }
    public void UpdateSocialMediaDetails(SocialMediaDetails NewSocialMediaDetails)
    {
        SocialMediaDetails = NewSocialMediaDetails;
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

    public void AddPet(Pet.Pet pet)
    {
        _pets.Add(pet);
        var positionNumber = PositionNumber.Create(CurrentPets.Count).Value;
        pet.SetPositionNumberToPet(positionNumber);
    }
    
    public Result<Pet.Pet, CustomError> GetPetById(Guid petId)
    {
        var pet = CurrentPets.FirstOrDefault(p => p.Id == petId);
        if (pet is null)
            return Errors.General.NotFound(petId);

        return pet;
    }
    
    private Result UpdatePetsPositions(List<Pet.Pet> orderedList)
    {
        if (_pets.Count < 2)
            return Result.Success();
        
        for (int i = 0; i < _pets.Count; i++)
        {
            var pet = orderedList[i];

            if (pet.PositionNumber.Value == i + 1)
                continue;
            
            var positionNumberResult = PositionNumber.Create(i + 1);

            if (positionNumberResult.IsFailure)
                return Result.Failure("Failed to update pets position");

            pet.SetPositionNumberToPet(positionNumberResult.Value);
        }
        
        return Result.Success();
    }

    public void ChangePetsPosition(Pet.Pet pet, int newPositionNumber)
    {
        var orderedList = _pets.OrderBy(x=>x.PositionNumber.Value).ToList();
        
        var currentPetsPosition = orderedList.IndexOf(pet);
        
        if (newPositionNumber >= 0 
            && newPositionNumber <= _pets.Count 
            && currentPetsPosition != newPositionNumber - 1)
        {
            orderedList.RemoveAt(currentPetsPosition);
            orderedList.Insert(newPositionNumber - 1, pet);
        }

        UpdatePetsPositions(orderedList);
    }
}