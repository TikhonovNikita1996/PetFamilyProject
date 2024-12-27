using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Core.Extensions;
using PetFamily.Species.Application.Database;

namespace PetFamily.Species.Application.Queries.GetSpecieByName;

public class GetSpecieByNameHandler : IQueryHandler<Result<SpecieDto, CustomErrorsList>,
    GetSpecieByNameQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetSpecieByNameQuery> _validator;

    public GetSpecieByNameHandler(
        IReadDbContext readDbContext,
        IValidator<GetSpecieByNameQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<SpecieDto, CustomErrorsList>> Handle(
        GetSpecieByNameQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesQuery = _readDbContext.Species;
        
        var specieDto = await speciesQuery.SingleOrDefaultAsync(v => v.Name == query.SpecieName
            ,cancellationToken);
        
        return specieDto;
    }
}