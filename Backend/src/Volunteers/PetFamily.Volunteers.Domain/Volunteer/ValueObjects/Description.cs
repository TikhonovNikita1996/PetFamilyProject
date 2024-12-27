using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Volunteers.Domain.Volunteer.ValueObjects;
public record Description
{
    private Description(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;
    public static Result<Description, CustomError> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description");
        
        var newDescription = new Description(description);

        return newDescription;
    }
}