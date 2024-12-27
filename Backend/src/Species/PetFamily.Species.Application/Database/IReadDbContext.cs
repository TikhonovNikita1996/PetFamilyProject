using PetFamily.Core.Dtos.Specie;

namespace PetFamily.Species.Application.Database;

public interface IReadDbContext
{
    IQueryable<SpecieDto> Species { get;}
    IQueryable<BreedDto> Breeds { get;}
}