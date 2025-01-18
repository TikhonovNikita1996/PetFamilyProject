using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<Result<RelationDto, CustomErrorsList>,
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

    public async Task<Result<RelationDto, CustomErrorsList>> Handle(
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