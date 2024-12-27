using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public class VolunteerContracts : IVolunteerContracts
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<VolunteerContracts> _logger;
    
    public VolunteerContracts(
        IReadDbContext readDbContext,
        ILogger<VolunteerContracts> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }
    
    public async Task<bool> IsAnyPetWithNeededSpecieExists(Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var petsWithSpecie = await _readDbContext.Pets
            .Select(p => p.SpecieId)
            .ToListAsync(cancellationToken);
        
        return petsWithSpecie.Contains(speciesId);
    }

    public async Task<bool> IsAnyPetWithNeededBreedExists(Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var petsWithSpecie = await _readDbContext.Pets
            .Select(p => p.BreedId)
            .ToListAsync(cancellationToken);
        
        return petsWithSpecie.Contains(breedId);
    }
}