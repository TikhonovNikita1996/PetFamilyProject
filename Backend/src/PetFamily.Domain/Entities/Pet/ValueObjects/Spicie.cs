using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;

public class Specie : BaseEntity<SpecieId>
{
    //For EF Core
    private Specie(SpecieId id) : base(id) {}

    private readonly List<Breed> _breeds = [];
    public string Name { get; private set; } = default!;
    public IReadOnlyList<Breed> Breeds => _breeds;
    private Specie(SpecieId id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Specie, CustomError> Create(SpecieId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(name);
        
        var species = new Specie(id, name);

        return species;
    }
    
    public Result<Guid, CustomError> AddBreed(Breed breed)
    {
        var result = _breeds.FirstOrDefault(b => b.Name == breed.Name);
        
        if (result is not null)
            return Errors.General.AlreadyExists(breed.Name);

        _breeds.Add(breed);

        return breed.Id.Value;
    }
    
    public Result<Guid, CustomError> DeleteBreed(Breed breed)
    {
        _breeds.Remove(breed);

        return breed.Id.Value;
    }
    
}