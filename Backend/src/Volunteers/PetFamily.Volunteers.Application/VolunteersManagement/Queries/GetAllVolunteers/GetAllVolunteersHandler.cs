using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetAllVolunteers;

public class GetAllVolunteersHandler : IQueryHandler<Result<PagedList<VolunteerDto>, CustomErrorsList>,
    GetAllVolunteersQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetAllVolunteersQuery> _validator;

    public GetAllVolunteersHandler(
        IReadDbContext readDbContext,
        IValidator<GetAllVolunteersQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<VolunteerDto>, CustomErrorsList>> Handle(
        GetAllVolunteersQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteersQuery = _readDbContext.Volunteers.AsQueryable();
        
        var pagedList = await volunteersQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);

        return pagedList;
    }
}