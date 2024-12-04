using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Pet.ValueObjects;
public record Color
{
    private Color(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;
    
    public static Result<Color, CustomError> Create(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return Errors.General.ValueIsInvalid("Color");
        
        var newColor = new Color(color);

        return newColor;
    }
}