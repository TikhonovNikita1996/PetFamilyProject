using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pet.Family.SharedKernel;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.SetMainPhoto;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.SoftDelete;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.MainInfo;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.Status;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPet;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPhotosToPet;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.ChangePetsPosition;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Create;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Delete;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.HardPetDelete;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.MainInfo;
using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetAllVolunteers;
using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetVolunteerById;
using PetFamily.Volunteers.Presentation.Processors;
using PetFamily.Volunteers.Presentation.Requests.Pet;
using PetFamily.Volunteers.Presentation.Requests.Volunteer;

namespace PetFamily.Volunteers.Presentation.Controllers;

[Authorize]
public class VolunteersController : BaseApiController
{
    [Permission(Permissions.Volunteers.Create)]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateVolunteerCommand(
            request.PhoneNumber,
            request.Description);
        
        var result = await createVolunteerHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Volunteers.Delete)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse(); 
            
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse(); 
            
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.Read)]
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
    
    [Permission(Permissions.Volunteers.Read)]
    [HttpGet("/volunteer-by-id")]
    public async Task<ActionResult> GetVolunteerById(
        [FromServices] GetVolunteerByIdHandler handler,
        [FromQuery] GetVolunteerByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result); 
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse(); 
            
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse(); 
            
        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.Delete)]
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
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Volunteers.Delete)]
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
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Volunteers.Update)]
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
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
}