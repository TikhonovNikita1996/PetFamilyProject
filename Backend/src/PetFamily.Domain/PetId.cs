namespace PetFamily.Domain;

public record PetId
{
    public Guid Value { get; }
    private PetId(Guid value)
    {
        Value = value;
    }
    
    public static PetId NewPetId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
}