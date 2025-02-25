using System.Text.Json;
using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Application.Database;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Caching;
using PetFamily.Core.Dtos.Account;
using PetFamily.Core.Extensions;

namespace PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;

public class GetUsersInfoHandler : IQueryHandler<Result<UserResponse, CustomErrorsList>,
    GetUsersInfoQuery>
{
    private readonly IAccountsReadDbContext _readDbContext;
    private readonly IValidator<GetUsersInfoQuery> _validator;
    private readonly IFileService _fileService;
    private readonly ICacheService _cache;
    
    public GetUsersInfoHandler(
        IAccountsReadDbContext readDbContext, 
        IValidator<GetUsersInfoQuery> validator,
        IFileService fileService, ICacheService cache)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _fileService = fileService;
        _cache = cache;
    }

    public async Task<Result<UserResponse, CustomErrorsList>> Handle(
        GetUsersInfoQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var cacheOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };
        
        string cacheKey = "user_" + query.UserId;
        
        var userResponse = await _cache.GetOrSetAsync(cacheKey,
            cacheOptions,
            async () => await GetUserById(query, cancellationToken), cancellationToken);
        
        if (userResponse is null)
            return Errors.General.NotFound("user").ToErrorList();
        
        return userResponse;
    }

    private async Task<UserResponse?> GetUserById(GetUsersInfoQuery query, CancellationToken cancellationToken)
    {
        var user = await _readDbContext.Users
            .Include(u => u.AdminAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.ParticipantAccount)
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user == null)
            return null;

        if (user.Photo != null)
        {
            var request = new GetFilePresignedUrlRequest([user.Photo!.FileId]);
            var urlResult = await _fileService.GetPresignedUrls(request, cancellationToken);
        }
        
        return new UserResponse
        {
            Id = user.Id,
            LastName = user.LastName,
            Name = user.Name,
            MiddleName = user.MiddleName,
            UserName = user.UserName,
            Email = user.Email,
            // PhotoId = user.Photo.FileId,
            // PhotoUrl = urlResult.Value.FirstOrDefault()!.PresignedUrl,
            AdminAccount = user.AdminAccount,
            VolunteerAccount = user.VolunteerAccount,
            ParticipantAccount = user.ParticipantAccount
        };
    }
}