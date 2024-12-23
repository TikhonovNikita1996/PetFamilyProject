﻿using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Contracts.Pet;
using PetFamily.API.Contracts.Volunteer;
using PetFamily.API.Processors;
using PetFamily.Application.Dtos;
using PetFamily.Application.Pets.SetMainPhoto;
using PetFamily.Application.Pets.SoftDelete;
using PetFamily.Application.Pets.Update.MainInfo;
using PetFamily.Application.Pets.Update.Status;
using PetFamily.Application.Queries.GetAllVolunteers;
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
            request.WorkingExperience,
            request.Email, request.PhoneNumber,
            request.Description, request.SocialMediaDetails,
            request.DonationInfo);
        
        var result = await createVolunteerHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateVolunteerMainInfoDto dto, 
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
    
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/position-number")]
    public async Task<ActionResult> ChangePetsPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChangePetsPositionRequest request,
        [FromServices] ChangePetsPositionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ChangePetsPositionCommand(volunteerId, petId, request.newPosition);
            
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error); 
            
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAllVolunteers(
        [FromServices] GetAllVolunteersHandler handler,
        [FromQuery] GetAllVolunteersRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result); 
    }
    
    [HttpGet("/volunteer-by-id")]
    public async Task<ActionResult> GetVolunteerById(
        [FromServices] GetVolunteerByIdHandler handler,
        [FromQuery] GetVolunteerByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        if(result is null)
            return BadRequest(Errors.General.NotFound(""));
        return Ok(result); 
    }
    
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/main-info")]
    public async Task<ActionResult> UpdatePetsMainInfo(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetsMainInfoDto dto,
        [FromServices] UpdatePetsMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePetsMainInfoCommand(volunteerId, petId, dto);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error); 
            
        return Ok(result.Value);
    }
    
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/status")]
    public async Task<ActionResult> UpdatePetsStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetsStatusRequest request,
        [FromServices] UpdatePetsStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePetsStatusCommand(volunteerId, petId, request.Status);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error); 
            
        return Ok(result.Value);
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult> SoftPetDelete(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] PetSoftDeleteHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new PetSoftDeleteCommand(volunteerId, petId);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> HardPetDelete(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardPetDeleteHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new HardPetDeleteCommand(volunteerId, petId);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/main-photo")]
    public async Task<ActionResult> SetPetMainPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] SetPetsMainPhotoHandler handler,
        [FromBody] SetPetsMainPhotoRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SetPetsMainPhotoCommand(volunteerId, petId, FilePath.Create(request.FilePath).Value);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value); 
    }
    
}