using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Database;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Account;
using PetFamily.Core.Extensions;

namespace PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;

public class GetUsersInfoHandler : IQueryHandler<Result<UserDto, CustomErrorsList>,
    GetUsersInfoQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetUsersInfoQuery> _validator;


    public GetUsersInfoHandler(
        IReadDbContext readDbContext, 
        IValidator<GetUsersInfoQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<UserDto, CustomErrorsList>> Handle(
        GetUsersInfoQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var userDto = await _readDbContext.Users
            .Include(u => u.AdminAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.ParticipantAccount)
            // .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        
        return userDto;
    }
}