using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects;

public record VolunteerInfo
{
    public string Value { get; }
    private VolunteerInfo(string value)
    {
        Value = value;
    }

    public static Result<VolunteerInfo, CustomError> Create(string value)
    {
        return new VolunteerInfo(value);
    } 
}