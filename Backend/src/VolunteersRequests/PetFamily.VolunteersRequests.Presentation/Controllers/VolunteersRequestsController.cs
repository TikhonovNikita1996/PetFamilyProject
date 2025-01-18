using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.VolunteersRequests.Application.Commands.CreateRequest;
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
}