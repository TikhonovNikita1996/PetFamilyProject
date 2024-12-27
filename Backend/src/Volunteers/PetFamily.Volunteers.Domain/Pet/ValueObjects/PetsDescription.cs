using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pet.ValueObjects;
public record PetsDescription
{
    private PetsDescription(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;
    public static Result<PetsDescription, CustomError> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description");
        
        var newDescription = new PetsDescription(description);

        return newDescription;
    }
}