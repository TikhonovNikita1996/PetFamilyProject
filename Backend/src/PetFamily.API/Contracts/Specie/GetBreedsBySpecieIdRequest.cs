namespace PetFamily.API.Contracts.Specie
{
    public record GetBreedsBySpecieIdRequest(
        int Page,
        int PageSize);
}