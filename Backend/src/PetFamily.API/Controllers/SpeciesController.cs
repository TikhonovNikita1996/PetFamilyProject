using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.Application.Dtos;
using PetFamily.Application.PetsSpecies.AddBreed;
using PetFamily.Application.PetsSpecies.Create;

namespace PetFamily.API.Controllers;

public class SpeciesController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpecieHandler handler,
        [FromBody] CreateSpecieRequest request,
        [FromServices] IValidator<CreateSpecieCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = new CreateSpecieCommand(request.Name, request.Breeds);
        
        var validationResult = await validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPost("{specieId:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromServices] AddBreedHandler handler,
        [FromRoute] Guid specieId,
        [FromBody] AddBreedRequest request,
        [FromServices] IValidator<AddBreedCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var command = new AddBreedCommand(specieId, request.Name);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
}