using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet;

public class Species : BaseEntity<SpeciesId>
{
    // ef core
    public Species(SpeciesId speciesId) : base(speciesId)
    {
        
    }

    public Species(SpeciesId speciesId, string name) : base(speciesId)
    {
        Name = name;
    }
    
    public string Name { get; private set; }

    public List<Breed> Breeds { get; set; }
    
}