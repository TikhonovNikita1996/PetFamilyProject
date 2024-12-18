using PetFamily.Application.Queries.GetAllSpecies;

namespace PetFamily.API.Contracts.Specie
{
    public record GetAllSpeciesRequest(
        int Page,
        int PageSize)
    {
        public GetAllSpeciesQuery ToQuery() =>
            new ( Page, PageSize);
    }
}