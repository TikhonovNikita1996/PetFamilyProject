namespace PetFamily.Domain.Entities.Pet;

public record Photos
{
    public List<PetPhoto> PetPhotos { get; private set; }
}