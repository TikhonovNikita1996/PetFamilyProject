using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ApproveRequest;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.CreateRequest;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.RejectRequest;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.ReopenRequest;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.SetRevisionRequiredStatus;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Commands.TakeInReview;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByAdminId;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllRequestsByUserId;
using PetFamily.VolunteersRequests.Application.VolunteersRequestsManagement.Queries.GetAllSubmuttedRequests;
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
        var command = new CreateRequestCommand(
            request.UserId,
            request.VolunteerInfo);
        
        var result = await createRequestHandler.Handle(command, cancellationToken);

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
        var command = new TakeInReviewCommand(
            adminId,
            request.RequestId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Update)]
    [HttpPut("{requestId:guid}/revision-required")]
    public async Task<ActionResult> SetRevisionRequiredStatus(
        [FromServices] SetRevisionRequiredStatusHandler handler,
        [FromBody] SetRejectionStatusRequest request,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var command = new SetRevisionRequiredStatusCommand(
            requestId,
            request.RejectionComment);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Update)]
    [HttpPut("{requestId:guid}/approve")]
    public async Task<ActionResult> SetApprovedStatus(
        [FromServices] ApproveRequestHandler handler,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var command = new ApproveRequestCommand(
            requestId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Create)]
    [HttpPut("{requestId:guid}/reopen")]
    public async Task<ActionResult> SetReopenStatus(
        [FromServices] ReopenRequestHandler handler,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var command = new ReopenRequestCommand(
            requestId);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Update)]
    [HttpPut("{requestId:guid}/reject")]
    public async Task<ActionResult> SetRejectStatus(
        [FromServices] RejectRequestHandler handler,
        [FromRoute] Guid requestId,
        [FromBody] SetRejectionStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RejectRequestCommand(
            requestId, request.RejectionComment);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission(Permissions.VolunteersRequests.Read)]
    [HttpGet("all-submutted-requests")]
    public async Task<ActionResult> GetAllSubmittedRequests(
        [FromServices] GetAllSubmittedRequestsHandler handler,
        [FromQuery] GetAllSubmittedRequestsRequest request, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllSubmittedRequestsQuery(request.Page, request.PageSize);
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.VolunteersRequests.Read)]
    [HttpGet("all-requests-by-admin")]
    public async Task<ActionResult> GetAllRequestsByAdminId(
        [FromServices] GetAllRequestsByAdminIdHandler handler,
        [FromQuery] GetAllRequestsByAdminIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllRequestsByAdminIdQuery(request.AdminId, request.Status,
            request.Page, request.PageSize);
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.VolunteersRequests.Read)]
    [HttpGet("all-requests-by-user")]
    public async Task<ActionResult> GetAllRequestsByUserId(
        [FromServices] GetAllRequestsByUserIdHandler handler,
        [FromQuery] GetAllRequestsByUserIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = new GetAllRequestsByUserIdQuery(request.UserId, request.Status,
            request.Page, request.PageSize);
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result.Value); 
    }
}