using CSharpFunctionalExtensions;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects;

public class VolunteerRequestId : ComparableValueObject
{
    private VolunteerRequestId(Guid value) => Value = value;
    public Guid Value { get; }
    public static VolunteerRequestId NewId() => new(Guid.NewGuid());
    public static VolunteerRequestId Empty() => new(Guid.Empty);
    public static VolunteerRequestId Create(Guid id) => new(id);
    
    public static implicit operator VolunteerRequestId(Guid id) => new(id);
    
    public static implicit operator Guid(VolunteerRequestId reqId)
    {
        ArgumentNullException.ThrowIfNull(reqId);

        return reqId.Value;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}