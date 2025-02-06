using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Contracts.Responses;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetsWithFiltersAndPagination;

public class GetPetsWithPaginationAndFiltersHandler : IQueryHandler<Result<PagedList<PetResponse>, CustomErrorsList>,
    GetPetsWithPaginationAndFiltersQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetPetsWithPaginationAndFiltersQuery> _validator;
    private readonly IFileService _fileService;

    public GetPetsWithPaginationAndFiltersHandler(
        IReadDbContext readDbContext,
        IValidator<GetPetsWithPaginationAndFiltersQuery> validator,
        IFileService fileService)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _fileService = fileService;
    }

    public async Task<Result<PagedList<PetResponse>, CustomErrorsList>> Handle(
        GetPetsWithPaginationAndFiltersQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var petsQuery = _readDbContext.Pets.AsQueryable();

        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => (pet) => pet.Name,
            "age" => (pet) => pet.Age,
            "specieId" => (pet) => pet.SpecieId,
            "breedId" => (pet) => pet.BreedId,
            "color" => (pet) => pet.Color,
            _ => (pet) => pet.Id
        };
        
        petsQuery = query.SortDirection?.ToLower() == "desc"
            ? petsQuery.OrderByDescending(keySelector)
            : petsQuery.OrderBy(keySelector);

        petsQuery = petsQuery.OrderBy(keySelector);
        
        petsQuery = petsQuery.WhereIf(query.VolunteerId != null,
            p => p.VolunteerId == query.VolunteerId);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Name),
            p => p.Name == query.Name);
        
        petsQuery = petsQuery.WhereIf(query.Age != null,
            p => p.Age == query.Age);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Gender),
            p => p.Gender == query.Gender);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Status),
            p => p.CurrentStatus == query.Status);
        
        petsQuery = petsQuery.WhereIf(query.SpeciesId != null,
            p => p.SpecieId == query.SpeciesId);
        
        petsQuery = petsQuery.WhereIf(query.BreedId != null,
            p => p.BreedId == query.BreedId);
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Color),
            p => p.Color == query.Color);
        
        var dtosPagedList = await petsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);

        List<Guid> photoIds = [];
        
        photoIds.AddRange(from dto in dtosPagedList.Items from photo in dto.Photos select photo.FileId);

        var request = new GetFilePresignedUrlRequest(photoIds);
        
        var urlsResult = await _fileService.GetPresignedUrls(request, cancellationToken);

        if (urlsResult.IsFailure)
            return Errors.General.NotFound("urls").ToErrorList();
        
        List<PetResponse> petResponses = [];

        foreach (var item in dtosPagedList.Items)
        {
            var photosIds = item.Photos.Select(photo => photo.FileId).ToList();
            List<string> photosUrls = [];
            
            photosUrls.AddRange(from fileResponse in urlsResult.Value 
                where photosIds.Contains(fileResponse.FileId)
                select fileResponse.PresignedUrl);

            var petResponse = PetResponse.ToPetResponse(item, photosIds, photosUrls);
            petResponses.Add(petResponse);
        }

        PagedList<PetResponse> pagedList = new PagedList<PetResponse>
        {
            Items = petResponses.AsReadOnly(),
            TotalCount = dtosPagedList.TotalCount,
            PageSize = dtosPagedList.PageSize,
            PageNumber = dtosPagedList.PageNumber
        };

        return pagedList;
    }
}