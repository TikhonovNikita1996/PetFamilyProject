namespace Pet.Family.SharedKernel.ValueObjects.Pet;

public record PhotosList
{
    public PhotosList() {}
    public IReadOnlyList<Photo> PetPhotos { get; }
    public PhotosList(List<Photo> photos) => PetPhotos = photos;
}