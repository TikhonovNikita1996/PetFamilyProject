namespace PetFamily.Domain.Entities.Pet;

public class Breed
{
    public Guid BreedId { get; private set; }
    public string Name { get; private set; }

    public Breed(Guid breedId, string name)
    {
        BreedId = breedId;
        Name = name;
    }
}