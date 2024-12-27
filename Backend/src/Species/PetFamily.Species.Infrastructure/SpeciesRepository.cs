using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Domain.ValueObjects;
using PetFamily.Species.Infrastructure.DataContexts;

namespace PetFamily.Species.Infrastructure;

public class SpeciesRepository(WriteDbContext context) : ISpeciesRepository
{
    public async Task<Result<Guid, CustomError>> Add(Specie specie, CancellationToken cancellationToken = default)
    {
        var result = await GetById(specie.Id, cancellationToken);
        if (result.IsSuccess)
        {
            return Errors.General.AlreadyExists(specie.Name);
        }
        
        await context.Species.AddAsync(specie, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return specie.Id.Value;
    }

    public async Task<Result<Guid>> Delete(Specie specie, CancellationToken cancellationToken = default)
    { 
        context.Species.Remove(specie);
        await context.SaveChangesAsync(cancellationToken);
        
        return specie.Id.Value;
    }

    public async Task<Result<Specie, CustomError>> GetById(SpecieId speciesId,
        CancellationToken cancellationToken = default)
    {
        var species = await context.Species
            .Include(v => v.Breeds)
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species is null)
            return Errors.General.NotFound(speciesId);

        return species;
    }
    
    public async Task<Result<Specie, CustomError>> GetByName(string name,
        CancellationToken cancellationToken = default)
    {
        var species = await context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        if (species is null)
            return Errors.General.NotFound("");
        return species;
    }
}