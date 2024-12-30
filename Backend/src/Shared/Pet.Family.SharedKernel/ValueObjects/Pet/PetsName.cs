using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Pet;

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