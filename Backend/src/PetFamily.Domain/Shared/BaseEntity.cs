namespace PetFamily.Domain.Shared;

public abstract class BaseEntity<T>(T id)
     where T : notnull
{
     public T Id { get; private set; } = id;
}