using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Queries.GetPetsWithFiltersAndPagination;

public class GetPetsWithPaginationAndFiltersHandler : IQueryHandler<PagedList<PetDto>,
    GetPetsWithPaginationAndFiltersQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetsWithPaginationAndFiltersHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetPetsWithPaginationAndFiltersQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _readDbContext.Pets.AsNoTracking();

        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => (pet) => pet.Name,
            "age" => (pet) => pet.Age,
            "specieId" => (pet) => pet.SpecieId,
            "breedId" => (pet) => pet.BreedId,
            "color" => (pet) => pet.Color,
            _ => (pet) => pet.Id
        };
        
        petsQuery = query.SortDirection?.ToLower() == "desc"
            ? petsQuery.OrderByDescending(keySelector)
            : petsQuery.OrderBy(keySelector);

        petsQuery = petsQuery.OrderBy(keySelector);
        
        petsQuery = petsQuery.WhereIf(query.VolunteerId != null,
            p => p.VolunteerId == query.VolunteerId);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Name),
            p => p.Name == query.Name);
        
        petsQuery = petsQuery.WhereIf(query.Age != null,
            p => p.Age == query.Age);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Gender),
            p => p.Gender == query.Gender);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Status),
            p => p.CurrentStatus == query.Status);
        
        petsQuery = petsQuery.WhereIf(query.SpeciesId != null,
            p => p.SpecieId == query.SpeciesId);
        
        petsQuery = petsQuery.WhereIf(query.BreedId != null,
            p => p.BreedId == query.BreedId);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Color),
            p => p.Color == query.Color);
        
        return await petsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}