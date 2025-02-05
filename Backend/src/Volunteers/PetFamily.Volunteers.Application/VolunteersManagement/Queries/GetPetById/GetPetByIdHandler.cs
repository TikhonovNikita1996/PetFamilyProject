using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Contracts.Responses;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<Result<PetResponse, CustomErrorsList>,
    GetPetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetPetByIdQuery> _validator;
    private readonly IFileService _fileService;


    public GetPetByIdHandler(
        IReadDbContext readDbContext, 
        IValidator<GetPetByIdQuery> validator,
        IFileService fileService)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _fileService = fileService;
    }

    public async Task<Result<PetResponse, CustomErrorsList>> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var speciesQuery = _readDbContext.Pets;
        
        var petDto = await speciesQuery.SingleOrDefaultAsync(v => v.Id == query.PetId
            ,cancellationToken);
        
        var photoIds = petDto!.Photos.Select(d => d.FileId).ToList();
        
        var request = new GetFilePresignedUrlRequest(photoIds);
        
        var urlsResult = await _fileService.GetPresignedUrls(request, cancellationToken);

        if (urlsResult.IsFailure)
            return Errors.General.NotFound("urls").ToErrorList();

        var photosIds = petDto.Photos.Select(p => p.FileId).ToList().AsReadOnly();
        var photosUrls = urlsResult.Value.Select(r => r.PresignedUrl).ToList().AsReadOnly();

        var petResponse = PetResponse.ToPetResponse(petDto, photosIds, photosUrls);
        
        return petResponse;
    }
}