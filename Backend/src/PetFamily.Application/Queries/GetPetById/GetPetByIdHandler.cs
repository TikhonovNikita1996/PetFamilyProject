using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PetDto> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var speciesQuery = _readDbContext.Pets;
        
        var petDto = await speciesQuery.SingleOrDefaultAsync(v => v.Id == query.PetId
            ,cancellationToken);
        
        return petDto;
    }
}