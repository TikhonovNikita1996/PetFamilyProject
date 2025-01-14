using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects;

public record RejectionComment
{
    public string Value { get; }
    private RejectionComment(string value)
    {
        Value = value;
    }

    public static Result<RejectionComment, CustomError> Create(string value)
    {
        return new RejectionComment(value);
    } 
}