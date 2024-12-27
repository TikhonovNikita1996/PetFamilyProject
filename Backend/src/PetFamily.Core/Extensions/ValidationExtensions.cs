using FluentValidation.Results;
using Pet.Family.SharedKernel;

namespace PetFamily.Core.Extensions;

public static class ValidationExtensions
{
    public static CustomErrorsList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = CustomError.Deserialize(errorMessage)
            select CustomError.Validation(error.Code, error.Message, validationError.PropertyName);

        return errors.ToList();
    }
}