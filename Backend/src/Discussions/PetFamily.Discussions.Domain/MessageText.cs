using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Discussions.Domain;

public record MessageText
{ 
    public string Value { get; }
    private MessageText(string value)
    {
        Value = value;
    }

    public static Result<MessageText, CustomError> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid("Message text");
        
        return new MessageText(value);
    } 

    public static implicit operator string(MessageText text) => text.Value;
}