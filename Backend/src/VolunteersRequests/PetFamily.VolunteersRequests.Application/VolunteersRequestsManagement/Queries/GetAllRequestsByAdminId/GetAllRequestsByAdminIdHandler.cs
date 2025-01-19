using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.VolunteersRequest;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteersRequests.Application.Database;
using PetFamily.VolunteersRequests.Domain.Enums;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByAdminId;

public class GetAllRequestsByAdminIdHandler : IQueryHandler<Result<PagedList<VolunteersRequestDto>, CustomErrorsList>,
    GetAllRequestsByAdminIdQuery>
{
    private readonly IVolunteersRequestReadDbContext _readDbContext;
    private readonly IValidator<GetAllRequestsByAdminIdQuery> _validator;

    public GetAllRequestsByAdminIdHandler(
        IVolunteersRequestReadDbContext readDbContext,
        IValidator<GetAllRequestsByAdminIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<VolunteersRequestDto>, CustomErrorsList>> Handle(
        GetAllRequestsByAdminIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var requestsQuery = _readDbContext.VolunteersRequests.WhereIf(true,
            x => x.Status == query.Status);
        
        requestsQuery = requestsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.AdminId.ToString()), x => x.AdminId == query.AdminId);
        
        requestsQuery = requestsQuery.WhereIf(
            string.IsNullOrWhiteSpace(query.AdminId.ToString()), x => x.Status == RequestStatus.OnReview.ToString());
        
        requestsQuery = requestsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.AdminId.ToString()), x => x.Status == query.Status);
        
        return await requestsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}