using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel;

public abstract class DomainEntity<TId> : Entity<TId>
    where TId : IComparable<TId>
{
    protected DomainEntity(TId id) : base(id)
    {
    }

    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}