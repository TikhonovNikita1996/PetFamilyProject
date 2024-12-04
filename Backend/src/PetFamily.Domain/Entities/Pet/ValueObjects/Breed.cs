using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;

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