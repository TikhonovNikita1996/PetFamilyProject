namespace PetFamily.Domain;

public class PetPhoto
{
    public Guid Id { get; private set; }
    public string Url { get; private set; } = default!;
    public bool IsMain { get; private set; } = default!;
}