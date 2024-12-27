using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    Task<SpecieDto?> GetSpecieByName(GetSpecieByNameRequest request,
        CancellationToken cancellationToken = default);
    
    Task<BreedDto?> GetBreedByName(GetBreedByNameRequest request,
        CancellationToken cancellationToken = default);
}