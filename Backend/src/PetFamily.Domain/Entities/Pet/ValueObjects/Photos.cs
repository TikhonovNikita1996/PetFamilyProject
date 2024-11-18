namespace PetFamily.Domain.Entities.Pet.ValueObjects;

public record Photos
{
    public List<PetPhoto> PetPhotos { get; private set; }
}