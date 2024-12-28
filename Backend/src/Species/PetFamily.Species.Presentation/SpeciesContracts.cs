using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation;

public class SpeciesContracts : ISpeciesContract
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<SpeciesContracts> _logger;
    
    public SpeciesContracts(
        IReadDbContext readDbContext,
        ILogger<SpeciesContracts> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }
    
    public async Task<SpecieDto?> GetSpecieByName(GetSpecieByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Species.FirstOrDefaultAsync(x => x.Name == request.SpecieName,
            cancellationToken);
    }

    public async Task<BreedDto?> GetBreedByName(GetBreedByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Breeds.FirstOrDefaultAsync(x => x.SpecieId == request.SpecieId
                                                        && x.Name == request.BreedName, cancellationToken);
    }
}