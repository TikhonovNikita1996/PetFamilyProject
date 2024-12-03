using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface ISpeciesRepository
{
    Task<Result<Guid, CustomError>> Add(Specie specie, CancellationToken cancellationToken = default);
    Task<Result<Guid>> Delete(Specie species, CancellationToken cancellationToken = default);
    Task<Result<Specie, CustomError>> GetById(SpecieId specieId, CancellationToken cancellationToken = default);
}