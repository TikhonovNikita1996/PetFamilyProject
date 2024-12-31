using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Pet;

public class HealthInformation
{
    private HealthInformation(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;
    
    public static Result<HealthInformation, CustomError> Create(string healthInformation)
    {
        if (string.IsNullOrWhiteSpace(healthInformation))
            return Errors.General.ValueIsInvalid("HealthInformation");
        
        var newPetsName = new HealthInformation(healthInformation);

        return newPetsName;
    }
}