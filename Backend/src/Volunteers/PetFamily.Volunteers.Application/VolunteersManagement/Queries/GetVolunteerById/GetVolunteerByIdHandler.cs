using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetVolunteerById;

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