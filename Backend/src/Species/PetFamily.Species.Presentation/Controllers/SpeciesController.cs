using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Species.Application.Commands.AddBreed;
using PetFamily.Species.Application.Commands.Create;
using PetFamily.Species.Application.Commands.DeleteBreed;
using PetFamily.Species.Application.Commands.DeleteSpecie;
using PetFamily.Species.Application.Queries.GetAllSpecies;
using PetFamily.Species.Application.Queries.GetBreedsBySpecieId;
using PetFamily.Species.Presentation.Specie;

namespace PetFamily.Species.Presentation.Controllers;

[Authorize]
public class SpeciesController : BaseApiController
{
    [Permission(Permissions.Species.Create)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpecieHandler handler,
        [FromBody] CreateSpecieRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSpecieCommand(request.Name, request.Breeds);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Breeds.Create)]
    [HttpPost("{specieId:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromServices] AddBreedHandler handler,
        [FromRoute] Guid specieId,
        [FromBody] AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AddBreedCommand(specieId, request.Name);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Species.Delete)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteSpecie(
        [FromRoute] Guid id,
        [FromServices] DeleteSpecieHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSpecieCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Breeds.Create)]
    [HttpDelete("{specieId:guid}/breed/{breedId:guid}")]
    public async Task<ActionResult> DeleteBreed(
        [FromRoute] Guid specieId,
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBreedCommand(specieId, breedId);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Species.Read)]
    [HttpGet]
    public async Task<ActionResult> GetAllSpecies(
        [FromServices] GetAllSpeciesHandler handler,
        [FromQuery] GetAllSpeciesRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result); 
    }
    
    [Permission(Permissions.Breeds.Read)]
    [HttpGet("{specieId:guid}/breeds")]
    public async Task<ActionResult> GetBreedsBySpecieId(
        [FromRoute] Guid specieId,
        [FromServices] GetBreedsBySpecieIdHandler handler,
        [FromQuery] GetBreedsBySpecieIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = new GetBreedsBySpecieIdQuery(specieId, request.Page, request.PageSize);
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result); 
    }
}