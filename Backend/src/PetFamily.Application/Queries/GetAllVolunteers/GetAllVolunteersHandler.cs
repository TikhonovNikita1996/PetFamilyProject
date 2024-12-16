using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Queries.GetAllVolunteers;

public class GetAllVolunteersHandler : IQueryHandler<PagedList<VolunteerDto>, GetAllVolunteersQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetAllVolunteersHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetAllVolunteersQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Volunteers.AsQueryable();
        
        volunteersQuery = volunteersQuery.WhereIf(
            query.VolunteerId is not null, p => p.Id == query.VolunteerId);
        
        var pagedList = await volunteersQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);

        return pagedList;
    }
}