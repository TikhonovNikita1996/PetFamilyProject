using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Pet;

public record PositionNumber
{
    private PositionNumber(int value)
    {
        Value = value;
    }
    public int Value { get; } = default!;
    
    public static Result<PositionNumber, CustomError> Create(int value)
     {
        if (value < 0)
            return Errors.General.ValueIsInvalid("position number");
        var newPositionNumber = new PositionNumber(value);

       return newPositionNumber;
    }
    
    public Result<PositionNumber, CustomError> Forward() =>
        Create(Value + 1);
    public Result<PositionNumber, CustomError> Back() =>
        Create(Value - 1);
    
    public static implicit operator int (PositionNumber position) =>
        position.Value;
}