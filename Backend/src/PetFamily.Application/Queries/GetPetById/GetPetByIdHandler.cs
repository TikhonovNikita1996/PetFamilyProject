using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<Result<PetDto, CustomErrorsList>,
    GetPetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetPetByIdQuery> _validator;


    public GetPetByIdHandler(
        IReadDbContext readDbContext, 
        IValidator<GetPetByIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PetDto, CustomErrorsList>> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesQuery = _readDbContext.Pets;
        
        var petDto = await speciesQuery.SingleOrDefaultAsync(v => v.Id == query.PetId
            ,cancellationToken);
        
        return petDto;
    }
}