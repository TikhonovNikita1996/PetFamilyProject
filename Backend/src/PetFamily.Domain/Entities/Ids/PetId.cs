namespace PetFamily.Domain.Entities.Ids;

public record PetId
{
    private PetId(Guid value) => Value = value;
    public Guid Value { get; }
    public static PetId NewVolonteerId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
    public static PetId Create(Guid id) => new(id);
}