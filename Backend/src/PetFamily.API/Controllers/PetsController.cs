using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Contracts.Pet;
using PetFamily.API.Contracts.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Dtos;
using PetFamily.Application.Pets.SetMainPhoto;
using PetFamily.Application.Pets.SoftDelete;
using PetFamily.Application.Pets.Update.MainInfo;
using PetFamily.Application.Pets.Update.Status;
using PetFamily.Application.Queries.GetAllVolunteers;
using PetFamily.Application.Queries.GetPetById;
using PetFamily.Application.Queries.GetPetsWithFiltersAndPagination;
using PetFamily.Application.Queries.GetVolunteerById;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPhotosToPet;
using PetFamily.Application.Volunteers.ChangePetsPosition;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.HardPetDelete;
using PetFamily.Application.Volunteers.Update.DonationInfo;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Application.Volunteers.Update.SocialMediaDetails;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Controllers;

public class PetsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> GetAllPets(
        [FromServices] GetPetsWithPaginationAndFiltersHandler handler,
        [FromQuery] GetPetsWithPaginationAndFiltersRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result); 
    }
    
    [HttpGet("/pet-by-id")]
    public async Task<ActionResult> GetPetById(
        [FromServices] GetPetByIdHandler handler,
        [FromQuery] GetPetByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result); 
    }
}