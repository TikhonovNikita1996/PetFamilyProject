using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet;

public record SpicieDetails
{
    public SpecieId SpecieId { get; }
    public Guid BreedId { get; }

    // ef
    public SpicieDetails()
    {
        
    }

    public SpicieDetails(SpecieId specieId, Guid breedId)
    {
        SpecieId = specieId;
        BreedId = breedId;
    }

    public static Result<SpicieDetails, CustomError> Create(SpecieId specieId, Guid breedId)
    {
        return new SpicieDetails(specieId, breedId);
    }
}