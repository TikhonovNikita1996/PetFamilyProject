using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.VolunteersRequests.Application.Commands.CreateRequest;
using PetFamily.VolunteersRequests.Application.Commands.SetRevisionRequiredStatus;
using PetFamily.VolunteersRequests.Application.Commands.TakeInReview;
using PetFamily.VolunteersRequests.Presentation.Requests;

namespace PetFamily.VolunteersRequests.Presentation.Controllers;

[Authorize]
public class VolunteersRequestsController : BaseApiController
{
    [Permission(Permissions.VolunteersRequests.Create)]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateRequestHandler createRequestHandler,
        [FromBody] CreateVolunteerRequestRequest request,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateRequestCommand(
            request.UserId,
            request.VolunteerInfo);
        
        var result = await createRequestHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Update)]
    [HttpPut("{adminId:guid}/take-in-review")]
    public async Task<ActionResult> TakeInReview(
        [FromServices] TakeInReviewHandler handler,
        [FromBody] TakeInReviewRequest request,
        [FromRoute] Guid adminId,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new TakeInReviewCommand(
            adminId,
            request.RequestId);
        
        var result = await handler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Update)]
    [HttpPut("{requestId:guid}/revision-required")]
    public async Task<ActionResult> SetRevisionRequiredStatus(
        [FromServices] SetRejectionStatusHandler handler,
        [FromBody] SetRejectionStatusRequest request,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new SetRejectionStatusCommand(
            requestId,
            request.RejectionComment);
        
        var result = await handler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}