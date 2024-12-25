using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Queries.GetAllVolunteers;

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