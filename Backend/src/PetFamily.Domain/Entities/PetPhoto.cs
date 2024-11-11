namespace PetFamily.Domain.Entities;

public record PetPhoto
{
    public string FilePath { get; private set; } = default!;
    public bool IsMain { get; private set; }
}