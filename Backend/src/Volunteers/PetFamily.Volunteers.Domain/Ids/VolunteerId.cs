namespace PetFamily.Volunteers.Domain.Ids;

public record VolunteerId
{
    private VolunteerId(Guid value) => Value = value;
    public Guid Value { get; }
    public static VolunteerId NewId() => new(Guid.NewGuid());
    public static VolunteerId Empty() => new(Guid.Empty);
    public static VolunteerId Create(Guid id) => new(id);
    
    public static implicit operator VolunteerId(Guid id) => new(id);

    public static implicit operator Guid(VolunteerId volunteerId)
    {
        ArgumentNullException.ThrowIfNull(volunteerId);

        return volunteerId.Value;
    }
}