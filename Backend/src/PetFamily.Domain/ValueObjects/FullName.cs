using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record FullName
{
    private FullName(string lastName,
        string name,
        string middleName)
    {
        LastName = lastName;
        Name = name;
        MiddleName = middleName;
    }
    public string LastName { get; } = default!;
    public string Name { get; } = default!;
    public string MiddleName { get; } = default!;
    
    public static Result<FullName, CustomError> Create(string lastName,
        string name,
        string middleName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsInvalid("Lastname");
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("Firstname");
        if (string.IsNullOrWhiteSpace(middleName))
            return Errors.General.ValueIsInvalid("Middlename");

        var newFullName = new FullName(lastName, name, middleName);

        return newFullName;
    }
}