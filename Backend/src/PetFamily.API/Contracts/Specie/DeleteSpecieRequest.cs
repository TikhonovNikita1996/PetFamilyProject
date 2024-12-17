using PetFamily.Application.Dtos;
using PetFamily.Application.Queries.SpecieBreeds;

namespace PetFamily.API.Contracts.Specie;

public record DeleteSpecieRequest(Guid SpecieId)
{
    public GetSpecieByIdQuery ToQuery() =>
        new (SpecieId);
}
