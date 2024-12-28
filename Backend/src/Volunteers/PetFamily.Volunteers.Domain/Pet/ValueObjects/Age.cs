using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Volunteers.Domain.Pet.ValueObjects;
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