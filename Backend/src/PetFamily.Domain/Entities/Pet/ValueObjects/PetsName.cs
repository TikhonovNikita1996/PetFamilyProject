using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;

public class PetsName
{
    private PetsName(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;
    
    public static Result<PetsName, CustomError> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("Pets name");
        
        var newPetsName = new PetsName(name);

        return newPetsName;
    }
}