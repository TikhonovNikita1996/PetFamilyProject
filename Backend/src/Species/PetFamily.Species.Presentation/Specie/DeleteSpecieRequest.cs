using PetFamily.Species.Application.Queries.GetSpecieById;

namespace PetFamily.Species.Presentation.Specie;

public record DeleteSpecieRequest(Guid SpecieId)
{
    public GetSpecieByIdQuery ToQuery() =>
        new (SpecieId);
}
