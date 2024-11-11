namespace PetFamily.Domain;

public record VolunteerId
{
    public Guid Value { get; }
    private VolunteerId (Guid value)
    {
        Value = value;
    }
    
    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
    public static VolunteerId Empty() => new(Guid.Empty);
}