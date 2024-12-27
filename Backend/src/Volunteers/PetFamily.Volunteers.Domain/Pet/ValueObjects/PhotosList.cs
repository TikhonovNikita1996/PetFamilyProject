namespace PetFamily.Volunteers.Domain.Pet.ValueObjects;

public record PhotosList
{
    public PhotosList() {}
    public IReadOnlyList<PetPhoto> PetPhotos { get; }
    public PhotosList(List<PetPhoto> photos) => PetPhotos = photos;
}