using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pet.Family.SharedKernel;
using PetFamily.Core.Models;

namespace PetFamily.Framework;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this CustomError error)
    {
        int statusCode = GetStatusCodeForErrorType(error.Type);
        
        var envelope = Envelope.Failure(error.ToErrorList());
        
        return new ObjectResult(envelope) { StatusCode = statusCode };
    }
    
    public static ActionResult ToResponse(this CustomErrorsList errors)
    {
        if (!errors.Any())
        {
            return new ObjectResult(Envelope.Failure(errors))
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }

        var distinctErrorTypes = errors
            .Select(x => x.Type)
            .Distinct()
            .ToList();

        int statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForErrorType(distinctErrorTypes.First());

        var envelope = Envelope.Failure(errors);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode,
        };
    }
    
    private static int GetStatusCodeForErrorType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
}