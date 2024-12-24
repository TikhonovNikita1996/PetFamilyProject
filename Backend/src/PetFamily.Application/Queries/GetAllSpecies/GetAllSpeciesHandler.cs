using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Queries.GetAllSpecies;

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