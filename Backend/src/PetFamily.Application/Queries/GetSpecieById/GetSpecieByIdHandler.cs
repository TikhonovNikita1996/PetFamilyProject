using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Queries.GetSpecieById;

public class GetSpecieByIdHandler : IQueryHandler<Result<SpecieDto, CustomErrorsList>,
    GetSpecieByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetSpecieByIdQuery> _validator;

    public GetSpecieByIdHandler(
        IReadDbContext readDbContext,
        IValidator<GetSpecieByIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<SpecieDto, CustomErrorsList>> Handle(
        GetSpecieByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesQuery = _readDbContext.Species;
        
        var specieDto = await speciesQuery.SingleOrDefaultAsync(v => v.SpecieId == query.SpecieId
            ,cancellationToken);
        
        return specieDto;
    }
}