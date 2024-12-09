using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;
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