using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DataContexts;

namespace PetFamily.Infrastructure.Repositories;

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
        await context.Species.AddAsync(specie, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return specie.Id.Value;
    }

    public async Task<Result<Specie, CustomError>> GetById(SpecieId speciesId,
        CancellationToken cancellationToken = default)
    {
        var species = await context.Species
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
            return Errors.General.NotFound();
        return species;
    }
}