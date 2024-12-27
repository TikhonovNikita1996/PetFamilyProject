using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Specie;

public record SpecieDetails
{
    public SpecieId SpecieId { get; }
    public BreedId BreedId { get; }

    // ef
    public SpecieDetails() {}

    public SpecieDetails(SpecieId specieId, BreedId breedId)
    {
        SpecieId = specieId;
        BreedId = breedId;
    }

    public static Result<SpecieDetails, CustomError> Create(SpecieId specieId, BreedId breedId)
    {
        return new SpecieDetails(specieId, breedId);
    }
}