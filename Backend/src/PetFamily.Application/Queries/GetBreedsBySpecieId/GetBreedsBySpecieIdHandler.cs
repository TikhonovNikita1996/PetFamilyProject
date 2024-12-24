using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Queries.GetBreedsBySpecieId;

public class GetBreedsBySpecieIdHandler : IQueryHandler<Result<PagedList<BreedDto>,
    CustomErrorsList>, GetBreedsBySpecieIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetBreedsBySpecieIdQuery> _validator;

    public GetBreedsBySpecieIdHandler(
        IReadDbContext readDbContext, 
        IValidator<GetBreedsBySpecieIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<BreedDto>, CustomErrorsList>> Handle(
        GetBreedsBySpecieIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
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