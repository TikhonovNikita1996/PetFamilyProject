namespace Pet.Family.SharedKernel;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
}