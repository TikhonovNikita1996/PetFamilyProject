using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Application.Database;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionById;

public class GetDiscussionByIdHandler : IQueryHandler<Result<DiscussionDto, CustomErrorsList>,
    GetDiscussionByIdQuery>
{
    private readonly IDiscussionsReadDbContext _readDbContext;
    private readonly IValidator<GetDiscussionByIdQuery> _validator;

    public GetDiscussionByIdHandler(
        IDiscussionsReadDbContext readDbContext,
        IValidator<GetDiscussionByIdQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<DiscussionDto, CustomErrorsList>> Handle(
        GetDiscussionByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var discussionDto = _readDbContext.Discussions.FirstOrDefault(d => d.DiscussionId == query.DiscussionId);
        
        return discussionDto;
    }
}