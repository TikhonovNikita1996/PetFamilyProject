using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Specie;

namespace PetFamily.Species.Domain.ValueObjects;

public class Breed : BaseEntity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }
    
    public Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }
    public string Name { get; private set; }
    
}