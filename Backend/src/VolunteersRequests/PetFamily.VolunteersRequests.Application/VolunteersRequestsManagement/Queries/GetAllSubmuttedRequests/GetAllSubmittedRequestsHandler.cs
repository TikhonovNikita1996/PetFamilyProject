using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Dtos.VolunteersRequest;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteersRequests.Application.Database;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.Enums;

namespace PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllSubmuttedRequests;

public class GetAllSubmittedRequestsHandler : IQueryHandler<Result<PagedList<VolunteersRequestDto>, CustomErrorsList>,
    GetAllSubmittedRequestsQuery>
{
    private readonly IVolunteersRequestReadDbContext _readDbContext;
    private readonly IValidator<GetAllSubmittedRequestsQuery> _validator;

    public GetAllSubmittedRequestsHandler(
        IVolunteersRequestReadDbContext readDbContext,
        IValidator<GetAllSubmittedRequestsQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<PagedList<VolunteersRequestDto>, CustomErrorsList>> Handle(
        GetAllSubmittedRequestsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var requestsQuery = _readDbContext.VolunteersRequests.WhereIf(true,
            x => x.Status == RequestStatus.Submitted.ToString());
        
        return await requestsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}