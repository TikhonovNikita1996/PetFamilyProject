namespace Pet.Family.SharedKernel;

public abstract class BaseEntity<TId> where TId : notnull
{
    protected BaseEntity(TId id) => Id = id;

    protected BaseEntity()
    {
        throw new NotImplementedException();
    }

    public TId Id { get; private set; }
}