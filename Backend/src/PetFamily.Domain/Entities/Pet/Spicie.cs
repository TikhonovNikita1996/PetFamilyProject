using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet;

public class Specie : BaseEntity<SpecieId>
{
    //For EF Core
    private Specie(SpecieId id) : base(id)
    {
            
    }
    private readonly List<Breed> _breeds = [];
    public string Name { get; private set; } = default!;
    public IReadOnlyList<Breed> Breeds => _breeds;
    private Specie(SpecieId id, string name, List<Breed> breeds) : base(id)
    {
        Name = name;
        _breeds = breeds;
    }

    public static Result<Specie, CustomError> Create(SpecieId id, string name, List<Breed> breeds)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(name);
        if (breeds.Count == 0)
            return Errors.General.ValueIsInvalid("Breeds");

        var species = new Specie(id, name, breeds);

        return species;
    }
    
}