namespace PetFamily.Domain.Entities;

public record Photos
{
    public List<PetPhoto> PetPhotos { get; private set; }
}