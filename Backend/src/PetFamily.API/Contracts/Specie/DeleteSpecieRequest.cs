using PetFamily.Application.Dtos;
using PetFamily.Application.Queries.GetSpecieById;

namespace PetFamily.API.Contracts.Specie;

public record DeleteSpecieRequest(Guid SpecieId)
{
    public GetSpecieByIdQuery ToQuery() =>
        new (SpecieId);
}
