
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;

namespace PetFamily.Framework;

[ApiController]
[Route("api/[controller]")]

public class BaseApiController : ControllerBase
{ 
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
    
        return base.Ok(envelope);
    }
}