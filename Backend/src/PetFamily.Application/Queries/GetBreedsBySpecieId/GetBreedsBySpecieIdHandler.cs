using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Application.Queries.GetAllSpecies;

namespace PetFamily.Application.Queries.GetBreedsBySpecieId;

public class GetBreedsBySpecieIdHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpecieIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetBreedsBySpecieIdHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<BreedDto>> Handle(
        GetBreedsBySpecieIdQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Breeds.AsQueryable();
        
        volunteersQuery = volunteersQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.SpecieId.ToString()), x => x.SpecieId == query.SpecieId);
        
        var pagedList = await volunteersQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);

        return pagedList;
    }
}