using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetsWithFiltersAndPagination;

public class GetPetsWithPaginationAndFiltersHandler : IQueryHandler<Result<PagedList<PetDto>, CustomErrorsList>,
    GetPetsWithPaginationAndFiltersQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetPetsWithPaginationAndFiltersQuery> _validator;

    public GetPetsWithPaginationAndFiltersHandler(
        IReadDbContext readDbContext,
        IValidator<GetPetsWithPaginationAndFiltersQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<PetDto>, CustomErrorsList>> Handle(
        GetPetsWithPaginationAndFiltersQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var petsQuery = _readDbContext.Pets.AsQueryable();

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