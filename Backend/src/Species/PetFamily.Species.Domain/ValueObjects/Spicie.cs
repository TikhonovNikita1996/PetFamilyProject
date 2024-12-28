using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Specie;

namespace PetFamily.Species.Domain.ValueObjects;

public class Specie : BaseEntity<SpecieId>
{
    //For EF Core
    private Specie(SpecieId id) : base(id) {}

    private readonly List<Breed> _breeds = [];
    public string Name { get; private set; } = default!;
    public IReadOnlyList<Breed> Breeds => _breeds;
    private Specie(SpecieId id, string name, List<Breed>? breeds = null) : base(id)
    {
        Name = name;
        _breeds = breeds;
    }

    public static Result<Specie, CustomError> Create(SpecieId id, string name, List<Breed>? breeds = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid(name);
        
        var species = new Specie(id, name, breeds);

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
    
    public Result<Guid, CustomError> DeleteBreed(Guid breedId)
    {
        var result = _breeds.FirstOrDefault(b => b.Id == breedId);
        _breeds.Remove(result);

        return result.Id.Value;
    }
    
}