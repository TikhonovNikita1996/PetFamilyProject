using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.CreateVolunteer;

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
            request.FullName,request.Age, request.Gender,
            request.Birthday, request.WorkingExperience,
            request.Email, request.PhoneNumber,
            request.Description, request.SocialMediaDetails, request.DonationInfo);
        
        var result = await createVolunteerHandler.Handle(createCommand, cancellationToken);
        
        if(result.IsFailure)
            BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}