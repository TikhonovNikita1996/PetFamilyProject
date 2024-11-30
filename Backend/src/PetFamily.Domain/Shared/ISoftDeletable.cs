namespace PetFamily.Domain.Shared;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
}