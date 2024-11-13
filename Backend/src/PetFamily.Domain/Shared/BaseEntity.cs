namespace PetFamily.Domain.Shared;

public abstract class BaseEntity<TId> where TId : notnull
{
    protected BaseEntity(TId id) => Id = id;
    
    public TId Id { get; private set; }
}