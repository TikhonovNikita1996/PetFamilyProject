using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet;

public class Breed : BaseEntity<BreedId>
{
    public string Name { get; private set; }

    //ef core
    private Breed()
    {
        
    }
    
    public Breed(BreedId breedId, string name)
    {
        Name = name;
    }
    
    public static Result<Breed, CustomError> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid();

        var breed = new Breed(id, name);

        return breed;
    }
    
}