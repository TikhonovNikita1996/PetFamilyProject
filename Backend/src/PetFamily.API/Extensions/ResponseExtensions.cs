using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this CustomError error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var responseError = new ResponseError(error.Code, error.Message, null);
        
        var envelope = Envelope.Failure([responseError]);
        
        return new ObjectResult(envelope) { StatusCode = statusCode };
    }
    
    public static ActionResult ToValidationErrorResponse(this ValidationResult result)
    {
        if (result.IsValid)
            throw new InvalidOperationException("Result can not be succeed");
        
        var validationErrors = result.Errors;
        
        var responseErrors = from validationError in validationErrors 
            let errorMessage = validationError.ErrorMessage 
            let error = CustomError.DeSerialize(errorMessage) 
            select new ResponseError(error.Code, error.Message, validationError.PropertyName);

        var envelope = Envelope.Failure(responseErrors);
        
        return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
    }
}