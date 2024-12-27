using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Species.Application.Database;

namespace PetFamily.Species.Application.Queries.GetAllSpecies;

public class GetAllSpeciesHandler : IQueryHandler<PagedList<SpecieDto>, GetAllSpeciesQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetAllSpeciesHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<SpecieDto>> Handle(
        GetAllSpeciesQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Species;
        
        var pagedList = await volunteersQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);

        return pagedList;
    }
}