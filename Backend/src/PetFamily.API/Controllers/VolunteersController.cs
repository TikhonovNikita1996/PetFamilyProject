using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.API.Response;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPhotosToPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Update.DonationInfo;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Application.Volunteers.Update.SocialMediaDetails;
using PetFamily.Domain.Shared;
using UpdateMainInfoRequest = PetFamily.API.Contracts.UpdateMainInfoRequest;

namespace PetFamily.API.Controllers;

public class VolunteersController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateVolunteerCommand(
            request.FullName, request.Gender,
            request.Birthday, request.WorkingExperience,
            request.Email, request.PhoneNumber,
            request.Description, request.SocialMediaDetails, request.DonationInfo);
        
        var result = await createVolunteerHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateMainInfoDto dto, 
        CancellationToken cancellationToken)
    {
        var command = new UpdateMainInfoCommand(id, dto);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
    [HttpPut("{id:guid}/social-media-info")]
    public async Task<ActionResult> UpdateSocialMedia(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialMediaDetailsHandler handler,
        [FromBody] UpdateSocialNetworksDto dto, 
        CancellationToken cancellationToken)
    {
        var command = new UpdateSocialMediaDetailsCommand(id, dto);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
    [HttpPut("{id:guid}/donation-info")]
    public async Task<ActionResult> UpdateDonationInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateDonationInfoHandler handler,
        [FromBody] UpdateDonationInfoDto dto, 
        CancellationToken cancellationToken)
    {
        var command = new UpdateDonationInfoCommand(id, dto);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid id,
        [FromServices] AddPetHandler handler,
        [FromBody] AddPetRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
    public async Task<ActionResult> UploadFilesToPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] AddPhotosToPetHandler handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);
            
        var command = new AddPhotosToPetCommand(volunteerId, petId, fileDtos);
            
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error); 
            
        return Ok(result.Value);
    }
}