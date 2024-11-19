using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Controllers;

public class VolunteersController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var createCommand = new CreateVolunteerCommand(
            request.FullName, request.Gender,
            request.Birthday, request.WorkingExperience,
            request.Email, request.PhoneNumber,
            request.Description, request.SocialMediaDetails, request.DonationInfo);
        
        var result = await createVolunteerHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}