using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Interfaces;

public interface ISpeciesRepository
{
    Task<Result<Guid, CustomError>> Add(Specie specie, CancellationToken cancellationToken = default);
    Task<Result<Guid>> Delete(Specie species, CancellationToken cancellationToken = default);
    Task<Result<Specie, CustomError>> GetById(SpecieId specieId, CancellationToken cancellationToken = default);
    Task<Result<Specie, CustomError>> GetByName(string specieName, CancellationToken cancellationToken = default);
}