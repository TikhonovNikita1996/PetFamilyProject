using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Core.Extensions;
using PetFamily.Species.Application.Database;

namespace PetFamily.Species.Application.Queries.GetBreedByName;

public class GetBreedByNameHandler : IQueryHandler<Result<BreedDto, CustomErrorsList>,
    GetBreedByNameQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetBreedByNameQuery> _validator;

    public GetBreedByNameHandler(
        IReadDbContext readDbContext,
        IValidator<GetBreedByNameQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<BreedDto, CustomErrorsList>> Handle(
        GetBreedByNameQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesQuery = _readDbContext.Breeds;
        
        var breedDto = await speciesQuery.SingleOrDefaultAsync(v => v.Name == query.BreedName
            ,cancellationToken);
        
        return breedDto;
    }
}