using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Account;
using PetFamily.Core.Extensions;

namespace PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;

public class GetUsersInfoHandler : IQueryHandler<Result<UserResponse, CustomErrorsList>,
    GetUsersInfoQuery>
{
    private readonly IAccountsReadDbContext _readDbContext;
    private readonly IValidator<GetUsersInfoQuery> _validator;
    private readonly IFileService _fileService;


    public GetUsersInfoHandler(
        IAccountsReadDbContext readDbContext, 
        IValidator<GetUsersInfoQuery> validator,
        IFileService fileService)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _fileService = fileService;
    }

    public async Task<Result<UserResponse, CustomErrorsList>> Handle(
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
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);
        
        if (userDto is null)
            return Errors.General.NotFound("user").ToErrorList();
        
        var request = new GetFilePresignedUrlRequest([userDto.Photo!.FileId]);
        var urlResult = await _fileService.GetPresignedUrls(request, cancellationToken);
        
        if (urlResult.IsFailure)
            return Errors.General.NotFound("url").ToErrorList();

        var userResponse = new UserResponse
        {
            Id = userDto.Id,
            LastName = userDto.LastName,
            Name = userDto.Name,
            MiddleName = userDto.MiddleName,
            UserName = userDto.UserName,
            Email = userDto.Email,
            PhotoId = userDto.Photo.FileId,
            PhotoUrl = urlResult.Value.FirstOrDefault()!.PresignedUrl,
            AdminAccount = userDto.AdminAccount,
            VolunteerAccount = userDto.VolunteerAccount,
            ParticipantAccount = userDto.ParticipantAccount
        };
        
        return userResponse;
    }
}