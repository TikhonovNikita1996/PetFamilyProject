using PetFamily.Species.Application.Queries.GetAllSpecies;

namespace PetFamily.Species.Presentation.Specie
{
    public record GetAllSpeciesRequest(
        int Page,
        int PageSize)
    {
        public GetAllSpeciesQuery ToQuery() =>
            new ( Page, PageSize);
    }
}