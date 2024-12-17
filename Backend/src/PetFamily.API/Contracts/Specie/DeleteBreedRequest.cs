using PetFamily.Application.Queries.SpecieBreeds;

namespace PetFamily.API.Contracts.Specie;

public record DeleteBreedByIdRequest(Guid SpecieId)
{
    public GetBreedByIdQuery ToQuery() =>
        new (SpecieId);
}
