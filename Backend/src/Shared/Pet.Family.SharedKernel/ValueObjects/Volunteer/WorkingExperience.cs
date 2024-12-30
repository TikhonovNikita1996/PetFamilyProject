using CSharpFunctionalExtensions;

namespace Pet.Family.SharedKernel.ValueObjects.Volunteer;
public record WorkingExperience
{
    private WorkingExperience(int value)
    {
        Value = value;
    }
    public int Value { get; } = default!;
    public static Result<WorkingExperience, CustomError> Create(int value)
    {
        if (value < 0)
            return Errors.General.DigitValueIsInvalid("Description");
        
        var newWorkingExperience = new WorkingExperience(value);

        return newWorkingExperience;
    }
}