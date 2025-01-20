using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.VolunteersRequest;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteersRequests.Application.Database;
using PetFamily.VolunteersRequests.Domain.Enums;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByUserId;

public class GetAllRequestsByUserIdHandler : IQueryHandler<Result<PagedList<VolunteersRequestDto>, CustomErrorsList>,
    GetAllRequestsByUserIdQuery>
{
    private readonly IVolunteersRequestReadDbContext _readDbContext;
    private readonly IValidator<GetAllRequestsByUserIdQuery> _validator;

    public GetAllRequestsByUserIdHandler(
        IVolunteersRequestReadDbContext readDbContext,
        IValidator<GetAllRequestsByUserIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<VolunteersRequestDto>, CustomErrorsList>> Handle(
        GetAllRequestsByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var requestsQuery = _readDbContext.VolunteersRequests.WhereIf(true,
            x => x.Status == query.Status);
        
        requestsQuery = requestsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.UserId.ToString()), x => x.UserId == query.UserId);
        
        requestsQuery = requestsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Status), x => x.Status == query.Status);
        
        return await requestsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}