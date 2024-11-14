using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Ids;

public class SpeciesId
{
    private SpeciesId(Guid value) => Value = value;
    public Guid Value { get; }
    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());
    public static SpeciesId Empty() => new(Guid.Empty);
    public static SpeciesId Create(Guid id) => new(id);
}