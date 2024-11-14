using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet;

public class Breed : BaseEntity<BreedId>
{
    public string Name { get; private set; }

    //ef 
    private Breed()
    {
        
    }
    
    public Breed(BreedId breedId, string name)
    {
        Name = name;
    }
    
    public static CustomResult<Breed> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Name can not be empty";

        var breed = new Breed(id, name);

        return breed;
    }
    
}