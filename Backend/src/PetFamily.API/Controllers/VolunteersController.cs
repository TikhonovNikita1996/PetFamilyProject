using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

public class VolunteersController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await createVolunteerHandler.Handle(createVolunteerRequest, cancellationToken);
        
        if(result.IsFailure)
            BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}