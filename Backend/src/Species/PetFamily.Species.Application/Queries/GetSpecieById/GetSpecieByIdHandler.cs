using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Core.Extensions;
using PetFamily.Species.Application.Database;

namespace PetFamily.Species.Application.Queries.GetSpecieById;

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