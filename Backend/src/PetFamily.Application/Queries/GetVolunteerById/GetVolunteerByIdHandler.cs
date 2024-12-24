using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<Result<VolunteerDto, CustomErrorsList>,
    GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetVolunteerByIdQuery> _validator;

    public GetVolunteerByIdHandler(
        IReadDbContext readDbContext,
        IValidator<GetVolunteerByIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<VolunteerDto, CustomErrorsList>> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteersQuery = _readDbContext.Volunteers;
        
        var volunteerDto = await volunteersQuery.SingleOrDefaultAsync(v => v.Id == query.VolunteerId
            ,cancellationToken);
        
        return volunteerDto;
    }
}