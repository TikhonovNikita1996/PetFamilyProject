using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Queries.GetSpecieById;

public class GetSpecieByIdHandler : IQueryHandler<SpecieDto, GetSpecieByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetSpecieByIdHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<SpecieDto> Handle(
        GetSpecieByIdQuery query,
        CancellationToken cancellationToken)
    {
        var speciesQuery = _readDbContext.Species;
        
        var specieDto = await speciesQuery.SingleOrDefaultAsync(v => v.SpecieId == query.SpecieId
            ,cancellationToken);
        
        return specieDto;
    }
}