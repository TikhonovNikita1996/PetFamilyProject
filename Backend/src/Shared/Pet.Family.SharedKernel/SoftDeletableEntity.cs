namespace Pet.Family.SharedKernel;

public class SoftDeletableEntity<TId> : BaseEntity<TId> where TId : notnull
{
    protected SoftDeletableEntity(TId id) : base(id)
    {
    }
    
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletionDate { get; protected set; }    
    
    public virtual void Delete()
    {
        if(IsDeleted) return;
        IsDeleted = true;
        DeletionDate = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletionDate = null;
    }
}