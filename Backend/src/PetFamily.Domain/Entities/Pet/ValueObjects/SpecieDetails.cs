using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;

public record SpecieDetails
{
    public SpecieId SpecieId { get; }
    public Guid BreedId { get; }

    // ef
    public SpecieDetails() {}

    public SpecieDetails(SpecieId specieId, Guid breedId)
    {
        SpecieId = specieId;
        BreedId = breedId;
    }

    public static Result<SpecieDetails, CustomError> Create(SpecieId specieId, Guid breedId)
    {
        return new SpecieDetails(specieId, breedId);
    }
}