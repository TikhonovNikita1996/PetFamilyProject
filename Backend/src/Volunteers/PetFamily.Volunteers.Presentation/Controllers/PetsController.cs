using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;
using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetsWithFiltersAndPagination;
using PetFamily.Volunteers.Presentation.Requests.Pet;

namespace PetFamily.Volunteers.Presentation.Controllers;

[Authorize]
public class PetsController : BaseApiController
{
    [HttpGet]
    [Permission(Permissions.Pets.Read)]
    public async Task<ActionResult> GetAllPets(
        [FromServices] GetPetsWithPaginationAndFiltersHandler handler,
        [FromQuery] GetPetsWithPaginationAndFiltersRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result); 
    }
    
    [Permission(Permissions.Pets.Read)]
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