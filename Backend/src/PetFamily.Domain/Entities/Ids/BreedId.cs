namespace PetFamily.Domain.Entities.Ids;

public record BreedId
{
    private BreedId(Guid value) => Value = value;
    public Guid Value { get; }
    public static BreedId NewId() => new(Guid.NewGuid());
    public static BreedId Empty() => new(Guid.Empty);
    public static BreedId Create(Guid id) => new(id);
    
    public static implicit operator Guid(BreedId breedId)
    {
        ArgumentNullException.ThrowIfNull(breedId);

        return breedId.Value;
    }
}