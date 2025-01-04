using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Volunteers.Domain.Ids;

namespace PetFamily.Volunteers.Domain.Volunteer;

public class Volunteer : SoftDeletableEntity<VolunteerId>
{
    private readonly List<Pet.Pet> _pets = [];
    
    // ef core
    public Volunteer(VolunteerId id) : base(id) {}

    public Volunteer(VolunteerId id,
        Description description,
        PhoneNumber phoneNumber) : base(id)
    {
        Description = description;
        PhoneNumber = phoneNumber;
    }
    public Description Description { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<Pet.Pet> CurrentPets => _pets;
    public int PetsWhoFoundHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.FoundHome);
    public int PetsSearchingForHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.SearchingForHome);
    public int PetsOnTreatment => _pets.Count(p => p.CurrentStatus == HelpStatusType.OnTreatment);
    
    public static Result<Volunteer, CustomError> Create(VolunteerId id,
        Description description, PhoneNumber phoneNumber)
    {
        
        var volunteer = new Volunteer(id, description, phoneNumber);

        return volunteer;
    }
    
    public void UpdateMainInfo(FullName fullName, 
        Description description, 
        PhoneNumber phoneNumber, 
        WorkingExperience workingExperience)
    {
        Description = description;
        PhoneNumber = phoneNumber;
    }
    
    public override void Delete()
    {
        base.Delete();
        
        foreach (var pet in _pets) {pet.Delete();}
    }
    
    public override void Restore()
    {
        base.Restore();
        foreach (var pet in _pets) {pet.Restore();}
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

    public Result UpdatePetsPositions(List<Pet.Pet> orderedList)
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

    public Result<Guid, CustomError> DeletePet(Pet.Pet pet)
    {
        if (pet == null)
            return Errors.General.NotFound(pet.Id.ToString());
        _pets.Remove(pet);
        
        return pet.Id.Value;
    }
}