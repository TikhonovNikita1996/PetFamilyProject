using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;
public record Age
{
    private Age(int value)
    {
        Value = value;
    }
    public int Value { get; } = default!;
    
    public static Result<Age, CustomError> Create(int age)
    {
        if (age < 0)
            return Errors.General.ValueIsInvalid("Age");
        
        var newAge = new Age(age);

        return newAge;
    }
}