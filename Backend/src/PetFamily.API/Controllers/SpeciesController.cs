using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.Application.Dtos;
using PetFamily.Application.PetsSpecies.AddBreed;

namespace PetFamily.API.Controllers;

public class SpeciesController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpeciesHandler handler,
        [FromBody] CreateSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("{specieId:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromRoute] Guid specieId,
        [FromServices] AddBreedHandler handler,
        [FromBody] AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new AddBreedCommand(specieId, request.Name);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}